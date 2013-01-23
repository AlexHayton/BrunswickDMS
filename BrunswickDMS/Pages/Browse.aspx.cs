using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrunswickDMS
{
    public partial class Browse : System.Web.UI.Page
    {
        DMSContext database = new DataLayer.Models.DMSContext();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Document> GetRecentDocuments()
        {
            IQueryable<Document> query = database.Documents;
            return query;
        }
    }
}