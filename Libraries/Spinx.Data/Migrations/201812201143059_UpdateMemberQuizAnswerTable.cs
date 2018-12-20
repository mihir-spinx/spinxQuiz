namespace Spinx.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMemberQuizAnswerTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MemberQuizAnswers", "MemberId", "dbo.Members");
            DropForeignKey("dbo.MemberQuizAnswers", "QuizId", "dbo.Quizs");
            DropIndex("dbo.MemberQuizAnswers", new[] { "MemberId" });
            DropIndex("dbo.MemberQuizAnswers", new[] { "QuizId" });
            AddColumn("dbo.MemberQuizAnswers", "MemberResultId", c => c.Int(nullable: false));
            AddColumn("dbo.MemberQuizAnswers", "SortOrder", c => c.Int());
            AddColumn("dbo.MemberResults", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.MemberResults", "EndTime", c => c.DateTime());
            CreateIndex("dbo.MemberQuizAnswers", "MemberResultId");
            AddForeignKey("dbo.MemberQuizAnswers", "MemberResultId", "dbo.MemberResults", "Id", cascadeDelete: true);
            DropColumn("dbo.MemberQuizAnswers", "MemberId");
            DropColumn("dbo.MemberQuizAnswers", "QuizId");
            DropColumn("dbo.MemberResults", "TimeDuration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberResults", "TimeDuration", c => c.String());
            AddColumn("dbo.MemberQuizAnswers", "QuizId", c => c.Int(nullable: false));
            AddColumn("dbo.MemberQuizAnswers", "MemberId", c => c.Int(nullable: false));
            DropForeignKey("dbo.MemberQuizAnswers", "MemberResultId", "dbo.MemberResults");
            DropIndex("dbo.MemberQuizAnswers", new[] { "MemberResultId" });
            DropColumn("dbo.MemberResults", "EndTime");
            DropColumn("dbo.MemberResults", "StartTime");
            DropColumn("dbo.MemberQuizAnswers", "SortOrder");
            DropColumn("dbo.MemberQuizAnswers", "MemberResultId");
            CreateIndex("dbo.MemberQuizAnswers", "QuizId");
            CreateIndex("dbo.MemberQuizAnswers", "MemberId");
            AddForeignKey("dbo.MemberQuizAnswers", "QuizId", "dbo.Quizs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MemberQuizAnswers", "MemberId", "dbo.Members", "Id", cascadeDelete: true);
        }
    }
}
