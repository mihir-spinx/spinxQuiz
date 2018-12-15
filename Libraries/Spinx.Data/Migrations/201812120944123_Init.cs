namespace Spinx.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        DisplayName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Left = c.Int(),
                        Right = c.Int(),
                        ParentId = c.Int(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdminPermissions", t => t.ParentId)
                .Index(t => t.Name, unique: true, name: "IX_AdminPermissionUniqueName")
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.AdminRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        SystemName = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SystemName, unique: true, name: "IX_AdminRoleUniqueSystemName");
            
            CreateTable(
                "dbo.AdminUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 256),
                        Salt = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        ForgotPasswordToken = c.String(maxLength: 256),
                        LastLoginAt = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "IX_AdminUserUniqueEmail");
            
            CreateTable(
                "dbo.ContactUsInquiries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 254),
                        Phone = c.String(maxLength: 20),
                        Details = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Slug = c.String(nullable: false, maxLength: 100),
                        Subject = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 150),
                        Password = c.String(nullable: false, maxLength: 100),
                        Salt = c.String(nullable: false, maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        ForgotPasswordToken = c.String(maxLength: 50),
                        CreatedSource = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        LastLoginAt = c.DateTime(),
                        AddressLine1 = c.String(nullable: false, maxLength: 256),
                        AddressLine2 = c.String(nullable: false, maxLength: 256),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Degree = c.String(nullable: false, maxLength: 100),
                        College = c.String(nullable: false, maxLength: 100),
                        LastSemMark = c.String(nullable: false, maxLength: 10),
                        Experience = c.String(nullable: false, maxLength: 100),
                        UploadResume = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "IX_MemberUniqueEmail");
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Slug = c.String(nullable: false, maxLength: 100),
                        Content = c.String(),
                        MetaTitle = c.String(maxLength: 100),
                        MetaDescription = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuizAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Answer = c.String(nullable: false, maxLength: 500),
                        IsCorrectAnswer = c.Boolean(nullable: false),
                        QuizQuestionId = c.Int(nullable: false),
                        SortOrder = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuizQuestions", t => t.QuizQuestionId, cascadeDelete: true)
                .Index(t => t.QuizQuestionId);
            
            CreateTable(
                "dbo.QuizQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(nullable: false, maxLength: 240),
                        QuizId = c.Int(nullable: false),
                        SortOrder = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .Index(t => t.QuizId);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 250),
                        Slug = c.String(nullable: false, maxLength: 250),
                        ShortDescription = c.String(nullable: false, maxLength: 400),
                        MetaTitle = c.String(maxLength: 100),
                        MetaDescription = c.String(maxLength: 500),
                        QuizCategoryId = c.Int(nullable: false),
                        SortOrder = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuizCategories", t => t.QuizCategoryId, cascadeDelete: true)
                .Index(t => t.QuizCategoryId);
            
            CreateTable(
                "dbo.QuizCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Slug = c.String(),
                        CategoryIcon = c.String(nullable: false, maxLength: 250),
                        SortOrder = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SeoPages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        MetaTitle = c.String(maxLength: 100),
                        MetaDescription = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_SeoPageUniqueName");
            
            CreateTable(
                "dbo.AdminRoleAdminPermissions",
                c => new
                    {
                        AdminRole_Id = c.Int(nullable: false),
                        AdminPermission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdminRole_Id, t.AdminPermission_Id })
                .ForeignKey("dbo.AdminRoles", t => t.AdminRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.AdminPermissions", t => t.AdminPermission_Id, cascadeDelete: true)
                .Index(t => t.AdminRole_Id)
                .Index(t => t.AdminPermission_Id);
            
            CreateTable(
                "dbo.AdminUserAdminRoles",
                c => new
                    {
                        AdminUser_Id = c.Int(nullable: false),
                        AdminRole_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdminUser_Id, t.AdminRole_Id })
                .ForeignKey("dbo.AdminUsers", t => t.AdminUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.AdminRoles", t => t.AdminRole_Id, cascadeDelete: true)
                .Index(t => t.AdminUser_Id)
                .Index(t => t.AdminRole_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuizAnswers", "QuizQuestionId", "dbo.QuizQuestions");
            DropForeignKey("dbo.QuizQuestions", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Quizs", "QuizCategoryId", "dbo.QuizCategories");
            DropForeignKey("dbo.AdminPermissions", "ParentId", "dbo.AdminPermissions");
            DropForeignKey("dbo.AdminUserAdminRoles", "AdminRole_Id", "dbo.AdminRoles");
            DropForeignKey("dbo.AdminUserAdminRoles", "AdminUser_Id", "dbo.AdminUsers");
            DropForeignKey("dbo.AdminRoleAdminPermissions", "AdminPermission_Id", "dbo.AdminPermissions");
            DropForeignKey("dbo.AdminRoleAdminPermissions", "AdminRole_Id", "dbo.AdminRoles");
            DropIndex("dbo.AdminUserAdminRoles", new[] { "AdminRole_Id" });
            DropIndex("dbo.AdminUserAdminRoles", new[] { "AdminUser_Id" });
            DropIndex("dbo.AdminRoleAdminPermissions", new[] { "AdminPermission_Id" });
            DropIndex("dbo.AdminRoleAdminPermissions", new[] { "AdminRole_Id" });
            DropIndex("dbo.SeoPages", "IX_SeoPageUniqueName");
            DropIndex("dbo.Quizs", new[] { "QuizCategoryId" });
            DropIndex("dbo.QuizQuestions", new[] { "QuizId" });
            DropIndex("dbo.QuizAnswers", new[] { "QuizQuestionId" });
            DropIndex("dbo.Members", "IX_MemberUniqueEmail");
            DropIndex("dbo.AdminUsers", "IX_AdminUserUniqueEmail");
            DropIndex("dbo.AdminRoles", "IX_AdminRoleUniqueSystemName");
            DropIndex("dbo.AdminPermissions", new[] { "ParentId" });
            DropIndex("dbo.AdminPermissions", "IX_AdminPermissionUniqueName");
            DropTable("dbo.AdminUserAdminRoles");
            DropTable("dbo.AdminRoleAdminPermissions");
            DropTable("dbo.SeoPages");
            DropTable("dbo.QuizCategories");
            DropTable("dbo.Quizs");
            DropTable("dbo.QuizQuestions");
            DropTable("dbo.QuizAnswers");
            DropTable("dbo.Pages");
            DropTable("dbo.Members");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.ContactUsInquiries");
            DropTable("dbo.AdminUsers");
            DropTable("dbo.AdminRoles");
            DropTable("dbo.AdminPermissions");
        }
    }
}
