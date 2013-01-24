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
            if (string.IsNullOrEmpty(Request.Form["AutoCompleteText"]))
            {
                this.SearchBox.SearchTerm.Value = string.Empty;
            }
            else
            {
                this.SearchBox.SearchTerm.Value = Request.Form["AutoCompleteText"];
                this.SearchListView.SearchTerm = Request.Form["AutoCompleteText"];
            }
        }
    }
}