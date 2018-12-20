namespace Spinx.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateMemberQuizAnswerAndMemberResultTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberQuizAnswers", "QuizQuestionId", c => c.Int(nullable: false));
            AddColumn("dbo.MemberResults", "QuizId", c => c.Int(nullable: false));
            CreateIndex("dbo.MemberQuizAnswers", "QuizQuestionId");
            CreateIndex("dbo.MemberResults", "QuizId");
            AddForeignKey("dbo.MemberQuizAnswers", "QuizQuestionId", "dbo.QuizQuestions", "Id", cascadeDelete: false);
            AddForeignKey("dbo.MemberResults", "QuizId", "dbo.Quizs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberResults", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.MemberQuizAnswers", "QuizQuestionId", "dbo.QuizQuestions");
            DropIndex("dbo.MemberResults", new[] { "QuizId" });
            DropIndex("dbo.MemberQuizAnswers", new[] { "QuizQuestionId" });
            DropColumn("dbo.MemberResults", "QuizId");
            DropColumn("dbo.MemberQuizAnswers", "QuizQuestionId");
        }
    }
}
