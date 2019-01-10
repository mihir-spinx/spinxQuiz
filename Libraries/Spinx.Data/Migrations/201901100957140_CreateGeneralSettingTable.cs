namespace Spinx.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateGeneralSettingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeneralSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Slug = c.String(nullable: false, maxLength: 150),
                        Name = c.String(nullable: false, maxLength: 100),
                        Value = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Slug, unique: true, name: "IX_GeneralSettingUniqueName");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.GeneralSettings", "IX_GeneralSettingUniqueName");
            DropTable("dbo.GeneralSettings");
        }
    }
}
