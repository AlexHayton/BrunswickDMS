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

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            // Bind the data on postback as well.
            this.DataBind();
        }

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

            // Bind the data on postback as well.
            this.DataBind();
        }

        public enum DocumentQueryMode
        {
            All,
            CurrentUser,
            DescendingDateOrder,
            Search
        }

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