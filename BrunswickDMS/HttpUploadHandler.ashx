﻿<%@ WebHandler Language="C#" Class="HttpUploadHandler" %>

using System;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using DataLayer.Models;
using UtilityFunctions;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * Modified by Alex Hayton to support saving to database.
 * */

public class HttpUploadHandler : IHttpHandler {

    private HttpContext _httpContext;
    private string _tempExtension = "_temp";
    private string _fileName;
    private string _parameters;
    private bool _lastChunk;
    private bool _firstChunk;
    private long _startByte;

    StreamWriter _debugFileStreamWriter;
    TextWriterTraceListener _debugListener;
       
    /// <summary>
    /// Start method
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        _httpContext = context;

        if (context.Request.InputStream.Length == 0)
        {
            throw new ArgumentException("No file input");
        }
        else
        {
            string tempPath = string.Empty;
            string uploadFolder = string.Empty;
            try
            {
                GetQueryStringParameters();

                uploadFolder = GetUploadFolder();
                string tempFileName = _fileName + _tempExtension;

                tempPath = GetTempFilePath(tempFileName);
            }
            catch (Exception e)
            {
                throw new FileNotFoundException("Cannot access temporary file path. See inner exception for more details.", e);
            }
            
            try
            {
                //Is it the first chunk? Prepare by deleting any existing files with the same name
                if (_firstChunk)
                {
                    Debug.WriteLine("First chunk arrived at webservice");

                    //Delete temp file               
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);
                }

                //Write the file to the temporary file stream.
                Debug.WriteLine(string.Format("Write data to disk FOLDER: {0}", uploadFolder));

                using (FileStream fs = File.Open(tempPath, FileMode.Append))
                {
                    SaveFile(context.Request.InputStream, fs);
                    fs.Close();
                }

                Debug.WriteLine("Write data to disk SUCCESS");

                //Is it the last chunk? Then finish up...
                if (_lastChunk)
                {
                    Debug.WriteLine("Last chunk arrived");

                    //Finish stuff....
                    SaveDocumentToDatabase(tempPath, context.User.Identity.Name, _fileName, _parameters);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());

                throw;
            }
        }
    }

    /// <summary>
    /// Get the querystring parameters
    /// </summary>
    private void GetQueryStringParameters()
    {
        _fileName = _httpContext.Request.QueryString["file"];
        _parameters = _httpContext.Request.QueryString["param"];
        _lastChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["last"]) ? true : bool.Parse(_httpContext.Request.QueryString["last"]);
        _firstChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["first"]) ? true : bool.Parse(_httpContext.Request.QueryString["first"]);
        _startByte = string.IsNullOrEmpty(_httpContext.Request.QueryString["offset"]) ? 0 : long.Parse(_httpContext.Request.QueryString["offset"]); ;
    }

    /// <summary>
    /// Save the contents of the Stream to a file
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="fs"></param>
    private void SaveFile(Stream stream, FileStream fs)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            fs.Write(buffer, 0, bytesRead);
        }
    }

    protected virtual string GetUploadFolder()
    {
        string folder = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
        if (string.IsNullOrEmpty(folder))
            folder = "Upload";

        return folder;
    }

    protected string GetTempFilePath(string fileName)
    {
        return Path.Combine(@HostingEnvironment.ApplicationPhysicalPath, Path.Combine(GetUploadFolder(), fileName));
    }

    /// <summary>
    /// This part actually saves the document to the database.
    /// The data is saved first, then the metadata, then any tags last.
    /// It is done in one big transaction.
    /// </summary>
    /// <param name="tempFile">The path to the temporary file</param>
    /// <param name="userName">The username of the person who uploaded the file</param>
    /// <param name="fileName">The filename of the file that was uploaded</param>
    /// <param name="parameters">Any parameters that were sent alongside</param>
    protected virtual void SaveDocumentToDatabase(string tempFile, string userName, string fileName, string parameters)
    {
        try
        {
            using (DMSContext database = new DMSContext())
            {

                // Save the document data to the database. 
                // The limit of 10MB prevents huge files being uploaded here and associated issues with memory usage.
                // The document data goes in its own table, to speed up access for metadata.
                DocumentData newData = null;
                long fileSize = 0;
                try
                {
                    newData = new DocumentData();
                    byte[] fileData = File.ReadAllBytes(tempFile);
                    fileSize = fileData.LongLength;
                    newData.FileData = fileData;
                    database.DocumentDataFiles.Add(newData);
                }
                catch (Exception e)
                {
                    throw new FileNotFoundException("Could not access the temporary file to read into the database! See inner exception for more details.", e);
                }

                try
                {
                    if (newData != null)
                    {
                        // Find the user that uploaded the document.
                        // Link by foreign key.
                        var existingUser = database.Users.SingleOrDefault(
                            u => u.UserName == userName);
                        if (existingUser == null)
                        {
                            throw new ArgumentOutOfRangeException("The uploading user does not exist in the database!");
                        }

                        DocumentType docType = DocumentWrangler.GetDocumentTypeFromExtension(fileName);
                        string mimeType = MimeTypes.GetMimeFromFile(fileName, tempFile);

                        // Block executable types for security.
                        if (mimeType == "application/x-msdownload")
                        {
                            throw new ArgumentOutOfRangeException("Cannot upload executables to this system for security reasons!");
                        }

                        // We actually save the document metadata here.
                        Document newDocument = new Document();
                        newDocument.Author = existingUser;
                        newDocument.DocumentData = newData;
                        newDocument.Name = fileName;
                        newDocument.DocType = docType;
                        newDocument.MimeType = mimeType;
                        newDocument.DocSize = fileSize;
                        database.Documents.Add(newDocument);

                        // Scan the document for tags and add any that were found to this document.
                        Dictionary<string, DocumentTagLink> tagLinkList = SearchTags.ScanDocumentForTags(database, tempFile, newDocument.DocType, newDocument.MimeType);
                        foreach (DocumentTagLink tagLink in tagLinkList.Values)
                        {
                            tagLink.Document = newDocument;
                            database.DocumentTagLinks.Add(tagLink);
                        }

                        // Commit.
                        database.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    throw new FileNotFoundException("Could not save the document into the database! See inner exception for more details.", e);
                }
            }
        }
        finally
        {
            // Clean up the temp file regardless of success or failure.
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}