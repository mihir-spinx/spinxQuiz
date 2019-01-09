namespace Spinx.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLengthinQuestiontable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuizQuestions", "Question", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuizQuestions", "Question", c => c.String(nullable: false, maxLength: 240));
        }
    }
}
