using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging; 

namespace UtilityFunctions
{
    public class SearchTags
    {
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

        private static Dictionary<string, DocumentTagLink> ConvertWordsIntoTags(DMSContext database, string plainText)
        {
            Dictionary<string, DocumentTagLink> tagsList = new Dictionary<string, DocumentTagLink>();
            plainText = plainText.ToUpperInvariant();
            List<String> words = new List<string>(plainText.Split(null as string[], StringSplitOptions.RemoveEmptyEntries));
            CleanUpWords(words);

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
        /// Removes any hanging full stops, commas etc. from the words.
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

                sb.Append(GetPlainText(element));

                // Use the results of the string builder to get the plain text.
                return sb.ToString();

            }
        }

        /// <summary> 
        ///  Read Plain Text in all XmlElements of word document 
        /// </summary> 
        /// <param name="element">XmlElement in document</param> 
        /// <returns>Plain Text in XmlElement</returns> 
        private static string GetPlainText(OpenXmlElement element)
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
                        PlainTextInWord.Append(GetPlainText(section));
                        PlainTextInWord.AppendLine(Environment.NewLine);
                        break;

                    default:
                        PlainTextInWord.Append(GetPlainText(section));
                        break;
                }
            }

            return PlainTextInWord.ToString();
        }
    }
}
