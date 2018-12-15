namespace Spinx.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMemberQuizAnswerAndMemberResultTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberQuizAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        QuizId = c.Int(nullable: false),
                        QuizAnswerId = c.Int(nullable: false),
                        IsRight = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .ForeignKey("dbo.QuizAnswers", t => t.QuizAnswerId, cascadeDelete: true)
                .Index(t => t.MemberId)
                .Index(t => t.QuizId)
                .Index(t => t.QuizAnswerId);
            
            CreateTable(
                "dbo.MemberResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        AttempedQues = c.Int(),
                        Score = c.Int(),
                        Percentage = c.Decimal(precision: 9, scale: 2),
                        TimeDuration = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberResults", "MemberId", "dbo.Members");
            DropForeignKey("dbo.MemberQuizAnswers", "QuizAnswerId", "dbo.QuizAnswers");
            DropForeignKey("dbo.MemberQuizAnswers", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.MemberQuizAnswers", "MemberId", "dbo.Members");
            DropIndex("dbo.MemberResults", new[] { "MemberId" });
            DropIndex("dbo.MemberQuizAnswers", new[] { "QuizAnswerId" });
            DropIndex("dbo.MemberQuizAnswers", new[] { "QuizId" });
            DropIndex("dbo.MemberQuizAnswers", new[] { "MemberId" });
            DropTable("dbo.MemberResults");
            DropTable("dbo.MemberQuizAnswers");
        }
    }
}
