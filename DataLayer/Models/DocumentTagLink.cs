using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    /// <summary>
    /// This class links documents to any relevant tags.
    /// </summary>
    public class DocumentTagLink : BaseData
    {
        public DocumentTagLink() : base()
        {
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DocumentTagLinkId { get; set; }

        [Required]
        public Document Document { get; set; }

        [Required]
        public Tag Tag { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
