namespace Spinx.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMeberTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "AddressLine1", c => c.String(maxLength: 256));
            AlterColumn("dbo.Members", "AddressLine2", c => c.String(maxLength: 256));
            AlterColumn("dbo.Members", "City", c => c.String(maxLength: 100));
            AlterColumn("dbo.Members", "State", c => c.String(maxLength: 100));
            AlterColumn("dbo.Members", "Phone", c => c.String(maxLength: 20));
            AlterColumn("dbo.Members", "Degree", c => c.String(maxLength: 100));
            AlterColumn("dbo.Members", "College", c => c.String(maxLength: 100));
            AlterColumn("dbo.Members", "LastSemMark", c => c.String(maxLength: 10));
            AlterColumn("dbo.Members", "Experience", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Experience", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Members", "LastSemMark", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Members", "College", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Members", "Degree", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Members", "Phone", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Members", "State", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Members", "City", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Members", "AddressLine2", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Members", "AddressLine1", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
