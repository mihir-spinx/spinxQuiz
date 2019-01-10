namespace Spinx.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMemberQuizAnswerOptionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberQuizAnswerOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberQuizAnswerId = c.Int(nullable: false),
                        QuizAnswerId = c.Int(),
                        SortOrder = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberQuizAnswers", t => t.MemberQuizAnswerId, cascadeDelete: true)
                .ForeignKey("dbo.QuizAnswers", t => t.QuizAnswerId)
                .Index(t => t.MemberQuizAnswerId)
                .Index(t => t.QuizAnswerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberQuizAnswerOptions", "QuizAnswerId", "dbo.QuizAnswers");
            DropForeignKey("dbo.MemberQuizAnswerOptions", "MemberQuizAnswerId", "dbo.MemberQuizAnswers");
            DropIndex("dbo.MemberQuizAnswerOptions", new[] { "QuizAnswerId" });
            DropIndex("dbo.MemberQuizAnswerOptions", new[] { "MemberQuizAnswerId" });
            DropTable("dbo.MemberQuizAnswerOptions");
        }
    }
}
