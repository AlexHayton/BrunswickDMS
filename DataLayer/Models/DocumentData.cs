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
    /// This class stores the actual data binary data for the documents
    /// We keep it in a separate table to improve performance.
    /// </summary>
    public class DocumentData : BaseData
    {
        public DocumentData() : base()
        {
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DocumentDataId { get; set; }

        [Required]
        public byte[] FileData { get; set; }
    }
}
