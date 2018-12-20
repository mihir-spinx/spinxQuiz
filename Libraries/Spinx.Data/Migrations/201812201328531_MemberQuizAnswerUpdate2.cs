namespace Spinx.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberQuizAnswerUpdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MemberQuizAnswers", "QuizAnswerId", "dbo.QuizAnswers");
            DropIndex("dbo.MemberQuizAnswers", new[] { "QuizAnswerId" });
            AlterColumn("dbo.MemberQuizAnswers", "QuizAnswerId", c => c.Int());
            AlterColumn("dbo.MemberQuizAnswers", "IsRight", c => c.Boolean());
            CreateIndex("dbo.MemberQuizAnswers", "QuizAnswerId");
            AddForeignKey("dbo.MemberQuizAnswers", "QuizAnswerId", "dbo.QuizAnswers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberQuizAnswers", "QuizAnswerId", "dbo.QuizAnswers");
            DropIndex("dbo.MemberQuizAnswers", new[] { "QuizAnswerId" });
            AlterColumn("dbo.MemberQuizAnswers", "IsRight", c => c.Boolean(nullable: false));
            AlterColumn("dbo.MemberQuizAnswers", "QuizAnswerId", c => c.Int(nullable: false));
            CreateIndex("dbo.MemberQuizAnswers", "QuizAnswerId");
            AddForeignKey("dbo.MemberQuizAnswers", "QuizAnswerId", "dbo.QuizAnswers", "Id", cascadeDelete: true);
        }
    }
}
