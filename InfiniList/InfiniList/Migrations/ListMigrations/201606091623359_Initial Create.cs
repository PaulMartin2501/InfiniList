namespace InfiniList.Migrations.ListMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Collections",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Title = c.String(nullable: false, maxLength: 65),
                        Format = c.String(nullable: false, maxLength: 50),
                        Size = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Lists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        Notes = c.String(maxLength: 100),
                        CollectionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Collections", t => t.CollectionID, cascadeDelete: true)
                .Index(t => t.CollectionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lists", "CollectionID", "dbo.Collections");
            DropIndex("dbo.Lists", new[] { "CollectionID" });
            DropTable("dbo.Lists");
            DropTable("dbo.Collections");
        }
    }
}
