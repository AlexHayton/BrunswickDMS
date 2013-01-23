using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    [Table("User")]
    public class User : BaseData
    {
        /// <summary>
        /// Set the user to be active here.
        /// </summary>
        public User() : base()
        {
            this.Active = true;
        }

        // Auto-increment the user id.
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        // Attributes for the users of the system.
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Set to false to deactivate a user.
        [Required]
        public bool Active { get; set; }
    }
}