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

        public bool CurrentUser { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Document> GetDocuments()
        {
            IQueryable<Document> query = null;
            if (CurrentUser)
            {
            }
            else
            {
                // Fallback : show all documents!
                query = database.Documents;
            }
            return query;
        }

        public IQueryable<Document> GetDocumentsForCurrentUser()
        {
            string userName = Request.LogonUserIdentity.Name;
            return (from d in database.Documents
                    where d.Author.UserName == userName
                    select d);
        }

        public IQueryable<Document> GetDocumentsBySearchTerm()
        {
            string userName = Request.LogonUserIdentity.Name;
            return (from d in database.Documents
                    where d.Author.UserName == userName
                    select d);
        }
    }
}