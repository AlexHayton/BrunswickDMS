using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityFunctions;

namespace BrunswickDMS
{
    public partial class DocumentListView : System.Web.UI.UserControl
    {
        DMSContext database = new DMSContext();

        private DocumentQueryMode _queryMode = DocumentQueryMode.All;

        /// <summary>
        /// Used to store the kind of query being run.
        /// </summary>
        public DocumentQueryMode QueryMode
        {
            get { return _queryMode; }
            set { _queryMode = value; }
        }

        private string _searchTerm = string.Empty;

        /// <summary>
        /// Used to store the search terms.
        /// </summary>
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // Bind the data on postback as well.
            this.DataBind();
        }

        /// <summary>
        /// Called whenever someone presses the 'Delete' button.
        /// We want to delete the correct row and then requery the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void delete_OnClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ListViewItem item = btn.NamingContainer as ListViewItem;
            Label documentIdField = item.FindControl("DocumentId") as Label;

            if (documentIdField != null)
            {
                int documentId = 0;
                bool success = int.TryParse(documentIdField.Text, out documentId);

                Document document = database.Documents
                                    .Where(d => d.DocumentId == documentId)
                                    .SingleOrDefault();

                List<DocumentTagLink> tagLinkList = database.DocumentTagLinks
                                                    .Include("Document")
                                                    .Where(l => l.Document.DocumentId == documentId)
                                                    .ToList();

                for (int i = tagLinkList.Count - 1; i >= 0; i--)
                {
                    database.DocumentTagLinks.Remove(tagLinkList[i]);
                }

                database.Documents.Remove(document);
                database.SaveChanges();
            }

            // Rebind to update the view.
            this.DataBind();
        }

        /// <summary>
        /// This enumeration is used to choose the mode of operation of this view.
        /// </summary>
        public enum DocumentQueryMode
        {
            All,
            CurrentUser,
            DescendingDateOrder,
            Search
        }

        /// <summary>
        /// This function delegates to the relevant function in DocumentWrangler.
        /// </summary>
        /// <returns>A queryable collection of documents that will be used in the view.</returns>
        public IQueryable<Document> GetDocuments()
        {
            IQueryable<Document> query = null;
            string userName = HttpContext.Current.User.Identity.Name;
            {
                switch (QueryMode)
                {
                    case DocumentQueryMode.CurrentUser:
                        query = DocumentWrangler.GetDocumentsForCurrentUser(database, userName);
                        break;

                    case DocumentQueryMode.DescendingDateOrder:
                        query = DocumentWrangler.GetDocumentsInDescendingDateOrder(database, userName);
                        break;

                    case DocumentQueryMode.Search:
                        // Don't try and search if the term is invalid.
                        if (string.IsNullOrWhiteSpace(SearchTerm))
                        {
                            query = database.Documents.Where(d => false);
                        }
                        else
                        {
                            query = DocumentWrangler.GetDocumentsBySearchTerm(database, userName, SearchTerm);
                        }
                        break;

                    case DocumentQueryMode.All:
                    default:
                        // Fallback : show all documents!
                        query = database.Documents.Include("Author");
                        break;
                }
            }
           
            return query;
        }

        /// <summary>
        /// This function converts a size in bytes to something a bit easier to read.
        /// </summary>
        /// <param name="fileSizeObject">The file size object.</param>
        /// <returns>A representation of the filesize e.g. "32MB"</returns>
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

        /// <summary>
        /// This function is used to get the download link for a given document.
        /// </summary>
        /// <param name="documentIdObject">The document ID object</param>
        /// <returns>A URL to the download link</returns>
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

        /// <summary>
        /// This function is used to get the icon for the document
        /// </summary>
        /// <param name="documentTypeObject">The type of the document</param>
        /// <param name="documentIdObject">The ID of the document</param>
        /// <returns>The URL to an icon suitable for showing this document</returns>
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