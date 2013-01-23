using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    [Table("Document")]
    public class Document : BaseData
    {
        public Document() : base()
        {
        }

        public enum DocumentType
        {
            Document,
            PDF,
            Presentation,
            Spreadsheet,
            Image,
            Unknown
        }

        public static DocumentType GetDocumentTypeFromExtension(string extension)
        {
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

        // Auto-increment the document ID value.
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DocumentId { get; set; }

        // Attributes for documents.
        [Required]
        public string Name { get; set; }
        [Required]
        public DocumentType DocType { get; set; }
        [Required]
        public User Author { get; set; }
        // The size of the file in bytes.
        [Required]
        public long DocSize { get; set; }

        // The document's data itself.
        // Store in a separate table to simplify query logic.
        public DocumentData DocumentData { get; set; }
    }
}