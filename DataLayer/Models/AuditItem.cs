using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    /// <summary>
    /// These were created for audit items. This functionality is unfinished.
    /// </summary>
    public class AuditItem : BaseData
    {
        public AuditItem() : base()
        {
        }

        public enum AuditType
        {
            Register,
            Login,
            Logout,
            AddDocument,
            RetrieveDocument,
            DeleteDocument
        }

        // Auto-increment the Audit Item value
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AuditId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public AuditType Action { get; set; }
    }
}