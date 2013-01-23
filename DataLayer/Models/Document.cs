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

        // Auto-increment a document ID value.
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DocumentId { get; set; }

        // Attributes for documents.
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public User Author { get; set; }

        // The document's data itself.
        // Store in a separate table to simplify query logic.
        public DocumentData DocumentData { get; set; }
    }
}