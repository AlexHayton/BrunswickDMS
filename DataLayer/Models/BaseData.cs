using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    /// <summary>
    /// This class provides some fields that are used everywhere, such as CreatedDate.
    /// </summary>
    public abstract class BaseData
    {
        /// <summary>
        /// Constructor to initialise default values for date fields.
        /// </summary>
        public BaseData()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}
