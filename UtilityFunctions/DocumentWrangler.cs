﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using System.IO;
using System.Web;

namespace UtilityFunctions
{
    /// <summary>
    /// This class contains utility functions for manipulating and querying documents.
    /// </summary>
    public class DocumentWrangler
    {
        /// <summary>
        /// This function gets the document type from its extension
        /// </summary>
        /// <param name="fileName">The filename of the document</param>
        /// <returns>DocumentType</returns>
        public static DocumentType GetDocumentTypeFromExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            DocumentType docType = DocumentType.Unknown;
            switch (extension)
            {
                case ".doc":
                case ".docx":
                    docType = DocumentType.Document;
                    break;

                case ".xls":
                case ".xlsx":
                    docType = DocumentType.Spreadsheet;
                    break;

                case ".ppt":
                case ".pptx":
                    docType = DocumentType.Presentation;
                    break;

                case ".pdf":
                    docType = DocumentType.PDF;
                    break;

                default:
                    docType = DocumentType.Unknown;
                    break;
            }

            return docType;
        }

        /// <summary>
        /// This returns a query that can be used to list documents for the current user.
        /// </summary>
        /// <param name="database">The database context</param>
        /// <param name="userName">The current username</param>
        /// <returns>A queryable object containing the data</returns>
        public static IQueryable<Document> GetDocumentsForCurrentUser(DMSContext database, string userName)
        {
            return database.Documents
                   .Include("Author")
                   .Where(d => d.Author.UserName == userName);
        }

        /// <summary>
        /// This returns a query that can be used to all documents in descending order.
        /// </summary>
        /// <param name="database">The database context</param>
        /// <param name="userName">The current username</param>
        /// <returns>A queryable object containing the data</returns>
        public static IQueryable<Document> GetDocumentsInDescendingDateOrder(DMSContext database, string userName)
        {
            return database.Documents
                   .Include("Author")
                   .OrderByDescending(d => d.CreatedDate)
                   .Where(d => (d.Private == false)
                                ||
                               (d.Private == true && d.Author.UserName == userName));
        }

        /// <summary>
        /// This returns a query that can be used to list documents by a given search term.
        /// </summary>
        /// <param name="database">The database context</param>
        /// <param name="userName">The current username</param>
        /// <param name="searchTerm">The search term</param>
        /// <returns>A queryable object containing the data</returns>
        public static IQueryable<Document> GetDocumentsBySearchTerm(DMSContext database, string userName, string searchTerm)
        {
            // TODO: I'd Prefer to use full text search here, if the database supports it!
            // Unfortunately SQL Azure and LocalDB both don't support this feature.
            // Instead, for now search based on document name and tags.
            searchTerm = searchTerm.ToUpperInvariant();
            
            // Construct a query to find all documents that contain the term in the name.
            IQueryable<Document> documents = (from d in database.Documents.Include("Author")
                                                 where (((d.Private == false)
                                                         ||
                                                         (d.Private == true && d.Author.UserName == userName))
                                                         &&
                                                         (d.Name.ToUpper().Contains(searchTerm)))
                                                 select d)
                                                 .Union(
            // Construct a query to find all documents that contain the term in a tag.
                                                (from d in database.Documents.Include("Author")
                                                from l in database.DocumentTagLinks
                                                    .Where(l => l.Document == d)
                                                    .DefaultIfEmpty()
                                                from t in database.Tags
                                                    .Where(t => t == l.Tag)
                                                    .DefaultIfEmpty()
                                                where (((d.Private == false)
                                                        ||
                                                        (d.Private == true && d.Author.UserName == userName))
                                                        &&
                                                        (t.TagName.ToUpper().Contains(searchTerm)))
                                                select d));

            return documents;
        }
    }
}
