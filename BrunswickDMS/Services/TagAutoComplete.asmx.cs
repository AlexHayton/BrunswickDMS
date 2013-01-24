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
    /// This web service allows the search box to autocomplete valid tags.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService] 
    public class TagAutoComplete : System.Web.Services.WebService
    {
        /// <summary>
        /// Given a search term and a limit on the number of results, this will
        /// find all the tags that match a given query.
        /// </summary>
        /// <param name="searchTerm">The term to be searched for</param>
        /// <param name="limit">A limit on the number of results</param>
        /// <returns>A list of results that can be parsed by the jQuery code.</returns>
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