using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using UtilityFunctions;

namespace BrunswickDMS.Services
{
    /// <summary>
    /// Summary description for TagAutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService] 
    public class TagAutoComplete : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TagResult> GetTags(string searchTerm, int limit)
        {
            List<TagResult> results = new List<TagResult>();

            using (DMSContext database = new DMSContext())
            {
                foreach (string foundTag in SearchTags.SearchForTags(database, searchTerm, limit))
                {
                    TagResult tagResult = new TagResult();
                    tagResult.Tag = foundTag;
                    results.Add(tagResult);
                }
            }

            return results;
        }
    }

    public class TagResult
    {
        public string Tag { get; set; }
    }
}