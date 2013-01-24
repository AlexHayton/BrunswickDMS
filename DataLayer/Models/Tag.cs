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
    /// This class allows tags from multiple documents to share common attributes.
    /// This could be used to make a Tag Cloud or something.
    /// </summary>
    public class Tag : BaseData
    {
        public Tag() : base()
        {
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }

        [Required]
        public string TagName { get; set; }

        public string Description { get; set; }
    }
}
