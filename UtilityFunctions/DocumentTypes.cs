using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using System.IO;

namespace UtilityFunctions
{
    public class DocumentTypes
    {
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
    }
}
