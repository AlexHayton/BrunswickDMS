using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using UtilityFunctions;

namespace BrunswickDMS
{
    /// <summary>
    /// Summary description for RetrieveDocument
    /// </summary>
    public class RetrieveDocument : IHttpHandler
    {
        // Re-use the database context here.
        DMSContext database = new DMSContext();

        public void ProcessRequest(HttpContext context)
        {
            // Check that the input is a number.
            int documentId = 0;
            bool validInput = int.TryParse(context.Request.QueryString["documentId"], out documentId);

            if (!validInput)
            {
                throw new ArgumentOutOfRangeException("Valid Document IDs must be a number");
            }

            // Find the document with the corresponding ID in the database.
            var documentMetadata = database.Documents
                        .Include("Author")
                        .Include("DocumentData")
                        .FirstOrDefault(d => d.DocumentId == documentId);

            // Abort if we could not find that document.
            if (documentMetadata == null)
            {
                throw new ArgumentOutOfRangeException("This is not a valid Document ID");
            }

            // Access control
            if (documentMetadata.Private)
            {
                if (documentMetadata.Author.UserName != context.User.Identity.Name)
                {
                    throw new UnauthorizedAccessException("This document is marked as private and you do not have access to view it!");
                }
            }

            // If we've passed all the previous checks, send the document.
            context.Response.ContentType = documentMetadata.MimeType;
            // Send a header for downloading the document if it can't be viewed in-place.
            if (!MimeTypes.CanBeInstantPreviewed(documentMetadata.MimeType))
            {
                context.Response.AddHeader("content-disposition", "attachment; filename=" + documentMetadata.Name);
            }
            byte[] bytesToSend = documentMetadata.DocumentData.FileData;
            using (BinaryWriter writer = new BinaryWriter(context.Response.OutputStream))
            {
                writer.Write(bytesToSend, 0, bytesToSend.Length);
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}