using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrunswickDMS
{
    public partial class DocumentListView : System.Web.UI.UserControl
    {
        DMSContext database = new DataLayer.Models.DMSContext();

        public DocumentQueryMode QueryMode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public enum DocumentQueryMode
        {
            All,
            CurrentUser,
            DescendingDateOrder
        }

        public IQueryable<Document> GetDocuments()
        {
            IQueryable<Document> query = null;
            switch (QueryMode)
            {
                case DocumentQueryMode.CurrentUser:
                    query = GetDocumentsForCurrentUser();
                    break;

                case DocumentQueryMode.DescendingDateOrder:
                    query = GetDocumentsInDescendingDateOrder();
                    break;

                case DocumentQueryMode.All:
                default:
                    // Fallback : show all documents!
                    query = database.Documents.Include("Author");
                    break;
            }
           
            return query;
        }

        public IQueryable<Document> GetDocumentsForCurrentUser()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            return database.Documents
                   .Include("Author")
                   .Where(d => d.Author.UserName == userName);
        }

        public IQueryable<Document> GetDocumentsInDescendingDateOrder()
        {
            return database.Documents
                   .Include("Author")
                   .OrderByDescending(d => d.CreatedDate);
        }


        public IQueryable<Document> GetDocumentsBySearchTerm()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            return (from d in database.Documents.Include("Author")
                    where ((d.Private == false) 
                           || 
                           (d.Private == true && d.Author.UserName == userName))
                    select d);
        }

        protected string GetHumanReadableFileSize(object fileSizeObject)
        {
            long? nullableFileSize = fileSizeObject as long?;
            string readableFileSize = string.Empty;

            if (nullableFileSize.HasValue)
            {
                long fileSize = nullableFileSize.Value;
                string[] suf = { "B", "KB", "MB", "GB", "TB", "PB" };
                int place = Convert.ToInt32(Math.Floor(Math.Log(fileSize, 1024)));
                double num = Math.Round(fileSize / Math.Pow(1024, place), 1);
                readableFileSize = num.ToString() + suf[place];
            }

            return readableFileSize;
        }

        protected string GetDownloadLinkForDocument(object documentIdObject)
        {
            int? documentId = documentIdObject as int?;
            string downloadLink = string.Empty;

            if (documentId.HasValue)
            {
                downloadLink = "/RetrieveDocument.ashx?documentId=" + documentId.Value;
            }
            return downloadLink;
        }

        protected string GetIconForDocument(object documentTypeObject, object documentIdObject)
        {
            DocumentType? docType = documentTypeObject as DocumentType?;
            int? documentId = documentIdObject as int?;
            
            string image = "/Images/FileTypes/unknown.png";
            // Only return something if we have a valid input.
            if (docType.HasValue && documentId.HasValue)
            {
                switch (docType.Value)
                {
                    case DocumentType.Document:
                        image = "/Images/FileTypes/word.png";
                        break;

                    case DocumentType.Spreadsheet:
                        image = "/Images/FileTypes/excel.png";
                        break;

                    case DocumentType.Presentation:
                        image = "/Images/FileTypes/powerpoint.png";
                        break;

                    case DocumentType.PDF:
                        image = "/Images/FileTypes/pdf.png";
                        break;

                    case DocumentType.Image:
                        image = GetDownloadLinkForDocument(documentId.Value);
                        break;

                    default:
                        image = "/Images/FileTypes/unknown.png";
                        break;
                }
            }

            return image;
        }
    }
}