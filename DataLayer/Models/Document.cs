using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public enum DocumentType
    {
        Document,
        PDF,
        Presentation,
        Spreadsheet,
        Image,
        Unknown
    }

    /// <summary>
    /// This class stores the metadata for documents.
    /// </summary>
    [Table("Document")]
    public class Document : BaseData
    {
        public Document() : base()
        {
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
        public string MimeType { get; set; }

        // Who uploaded this?
        [Required]
        public User Author { get; set; }

        // The size of the file in bytes.
        [Required]
        public long DocSize { get; set; }

        // Private documents can only be seen by the author.
        [Required]
        public bool Private { get; set; }

        // The document's data itself.
        // Store in a separate table to speed up queries and segregate metadata from binary.
        [Required]
        public DocumentData DocumentData { get; set; }
    }
}