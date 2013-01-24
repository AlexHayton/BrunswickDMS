using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrunswickDMS
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Handle postbacks here
            if (!IsPostBack)
            {
                SearchTerm.Value = string.Empty;
                SearchResultsDiv.Visible = false;
            }
            else
            {
                if (this.SearchTerm.Value.Length > 0)
                {
                    this.SearchResultsDiv.Visible = true;
                    this.SearchListView.SearchTerm = this.SearchTerm.Value;
                    this.SearchListView.DataBind();
                }
                else
                {
                    this.SearchResultsDiv.Visible = false;
                }
            }
        }
    }
}