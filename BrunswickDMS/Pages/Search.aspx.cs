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

        }

        /// <summary>
        /// Normally I would use full-text search against SQL Server here.
        /// However, neither LocalDB nor Windows Azure's SQL instances support this feature so I'll roll my own.
        /// </summary>
        protected void PerformSearch()
        {
            
        }
    }
}