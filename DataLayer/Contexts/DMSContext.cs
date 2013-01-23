using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public class DMSContext : DbContext
    {
        public DMSContext()
            : base("DMSDataConnection")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<AuditItem> AuditItems { get; set; }
        public DbSet<DocumentData> DocumentDataFiles { get; set; }
        public DbSet<DocumentTagLink> DocumentTagLinks { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}