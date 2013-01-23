<%@ WebHandler Language="C#" Class="HttpUploadHandler" %>

using System;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Diagnostics;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
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
            try
            {
                GetQueryStringParameters();

                string uploadFolder = GetUploadFolder();
                string tempFileName = _fileName + _tempExtension;

                string tempPath = GetTempFilePath(tempFileName);

                //Is it the first chunk? Prepare by deleting any existing files with the same name
                if (_firstChunk)
                {
                    Debug.WriteLine("First chunk arrived at webservice");

                    //Delete temp file               
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);
                }

                //Write the file to the database.
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

                    // Upload file contents to the database.

                    //Finish stuff....
                    FinishedFileUpload(_fileName, _parameters);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());

                throw;
            }
            finally
            {
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
    /// Do your own stuff here when the file is finished uploading
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="parameters"></param>
    protected virtual void FinishedFileUpload(string fileName, string parameters)
    {
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

    protected string GetTargetFilePath(string fileName)
    {
        return Path.Combine(@HostingEnvironment.ApplicationPhysicalPath, Path.Combine(GetUploadFolder(), fileName));
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}