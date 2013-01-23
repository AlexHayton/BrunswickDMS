namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrivateColumn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Document", "DocumentData_DocumentDataId", "dbo.DocumentDatas");
            DropIndex("dbo.Document", new[] { "DocumentData_DocumentDataId" });
            AddColumn("dbo.Document", "Private", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.Document", "DocumentData_DocumentDataId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Document", "DocumentData_DocumentDataId", "dbo.DocumentDatas", "DocumentDataId", cascadeDelete: true);
            CreateIndex("dbo.Document", "DocumentData_DocumentDataId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Document", new[] { "DocumentData_DocumentDataId" });
            DropForeignKey("dbo.Document", "DocumentData_DocumentDataId", "dbo.DocumentDatas");
            AlterColumn("dbo.Document", "DocumentData_DocumentDataId", c => c.Int());
            DropColumn("dbo.Document", "Private");
            CreateIndex("dbo.Document", "DocumentData_DocumentDataId");
            AddForeignKey("dbo.Document", "DocumentData_DocumentDataId", "dbo.DocumentDatas", "DocumentDataId");
        }
    }
}
