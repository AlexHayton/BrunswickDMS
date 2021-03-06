﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging; 

namespace UtilityFunctions
{
    /// <summary>
    /// A class containing utility functions for working with Tags.
    /// </summary>
    public class SearchTags
    {
        /// <summary>
        /// This function is used to return a list of tags that start with a given string
        /// Used for autocomplete on the search box.
        /// </summary>
        /// <param name="database">The database context</param>
        /// <param name="searchTerm">The search term that we're using</param>
        /// <param name="limit">The maximum number of tags to return</param>
        /// <returns>A queryable object containing the tags</returns>
        public static List<string> SearchForTags(DMSContext database, string searchTerm, int limit)
        {
            List<string> results = new List<string>();

            var tagList = (from t in database.Tags
                           join l in database.DocumentTagLinks on t equals l.Tag
                           where t.TagName.StartsWith(searchTerm)
                           group l by t.TagName into g
                           select new
                           {
                               Name = g.Key,
                               Sum = g.Sum(l => l.Count),
                           }).Take(limit);

            foreach (var foundTag in tagList)
            {
                results.Add(foundTag.Name);
            }

            return results;
        }

        /// <summary>
        /// Scans a document and extracts its tags in a format suitable for saving to the database.
        /// </summary>
        /// <param name="database">The database context</param>
        /// <param name="fileName">The filename of the document</param>
        /// <param name="docType">The document type of the document</param>
        /// <param name="mimeType">The Mime Type of the document</param>
        /// <returns>A list of tags that were returned, ready to insert into the database.</returns>
        public static Dictionary<string, DocumentTagLink> ScanDocumentForTags(DMSContext database, string fileName, DocumentType docType, string mimeType)
        {
            Dictionary<string, DocumentTagLink> tagsList = null;

            // Allow scanning of documents for text for search purposes.
            switch (mimeType)
            {
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.template":
                case "application/vnd.ms-word.document.macroEnabled.12":
                case "application/vnd.ms-word.template.macroEnabled.12":
                    string plainText = ExtractWordDocumentText(fileName);
                    tagsList = ConvertWordsIntoTags(database, plainText);
                    break;

                default:
                    tagsList = new Dictionary<string, DocumentTagLink>();
                    break;
            }

            return tagsList;
        }

        /// <summary>
        /// Given a list of words, this will convert each one into the appropriate Tag
        /// </summary>
        /// <param name="database">The database context</param>
        /// <param name="plainText">The plain text content of the document</param>
        /// <returns>A list of tags that were returned, ready to insert into the database.</returns>
        private static Dictionary<string, DocumentTagLink> ConvertWordsIntoTags(DMSContext database, string plainText)
        {
            Dictionary<string, DocumentTagLink> tagsList = new Dictionary<string, DocumentTagLink>();
            plainText = plainText.ToUpperInvariant();
            List<String> words = new List<string>(plainText.Split(null as string[], StringSplitOptions.RemoveEmptyEntries));
            CleanUpWords(words);
            
            // Loop over the list of words.
            // For each one, find a matching tag and create a new one if we couldn't.
            if (words.Count > 0)
            {
                foreach (string word in words)
                {
                    DocumentTagLink documentTagLink = null;

                    if (!tagsList.ContainsKey(word))
                    {
                        // Link to an existing tag if it exists, or create a new one.
                        Tag tag = database.Tags.SingleOrDefault(
                            t => t.TagName.ToUpper() == word.ToUpper());
                        if (tag == null)
                        {
                            tag = new Tag();
                            tag.TagName = word;
                            database.Tags.Add(tag);
                        }

                        documentTagLink = new DocumentTagLink();
                        documentTagLink.Tag = tag;
                        tagsList.Add(word, documentTagLink);
                    }
                    else
                    {
                        documentTagLink = tagsList[word];
                    }

                    // Increment the counter variable here.
                    documentTagLink.Count++;
                }
            }

            return tagsList;
        }

        /// <summary>
        /// Removes any hanging full stops, commas etc. from the words list.
        /// </summary>
        /// <param name="words"></param>
        private static void CleanUpWords(List<string> words)
        {
            for (int i = words.Count - 1; i >= 0; i--)
            {
                string word = words[i];
                words[i] = words[i].Trim();
                // Check the last character directly.
                switch (word[word.Length - 1])
                {
                    case '.':
                    case ',':
                    case ':':
                    case ';':
                    case '…':
                        words[i] = word.Remove(word.Length - 1);
                        break;

                    // Leave it alone by default.
                    default:
                        break;
                }

                // Remove any words that are now empty due to our processing...
                if (string.IsNullOrWhiteSpace(words[i]))
                {
                    words.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Use the Open XML SDK to open the word document and get its text.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string ExtractWordDocumentText(string fileName)
        {
            using (WordprocessingDocument package = WordprocessingDocument.Open(fileName, true))
            {
                StringBuilder sb = new StringBuilder();
                OpenXmlElement element = package.MainDocumentPart.Document.Body;
                if (element == null)
                {
                    return string.Empty;
                }

                sb.Append(GetWordPlainText(element));

                // Use the results of the string builder to get the plain text.
                return sb.ToString();

            }
        }

        /// <summary> 
        /// Read Plain Text in all XmlElements of word document 
        /// </summary> 
        /// <param name="element">XmlElement in document</param> 
        /// <returns>Plain Text in XmlElement</returns> 
        private static string GetWordPlainText(OpenXmlElement element)
        {
            StringBuilder PlainTextInWord = new StringBuilder();
            foreach (OpenXmlElement section in element.Elements())
            {
                switch (section.LocalName)
                {
                    // Text 
                    case "t":
                        PlainTextInWord.Append(section.InnerText);
                        break;

                    case "cr":                          // Carriage return 
                    case "br":                          // Page break 
                        PlainTextInWord.Append(Environment.NewLine);
                        break;

                    // Tab 
                    case "tab":
                        PlainTextInWord.Append("\t");
                        break;

                    // Paragraph 
                    case "p":
                        PlainTextInWord.Append(GetWordPlainText(section));
                        PlainTextInWord.AppendLine(Environment.NewLine);
                        break;

                    default:
                        PlainTextInWord.Append(GetWordPlainText(section));
                        break;
                }
            }

            return PlainTextInWord.ToString();
        }
    }
}
