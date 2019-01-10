using Spinx.Core.Domain;
using Spinx.Data.Configuration.AdminRolePermissions;
using Spinx.Data.Configuration.AdminUsers;
using Spinx.Data.Configuration.ContactUsInquiries;
using Spinx.Data.Configuration.EmailTemplates;
using Spinx.Data.Configuration.Members;
using Spinx.Data.Configuration.Pages;
using Spinx.Data.Configuration.QuizAnswers;
using Spinx.Data.Configuration.QuizCategories;
using Spinx.Data.Configuration.QuizQuestions;
using Spinx.Data.Configuration.Quizs;
using Spinx.Data.Configuration.SeoPages;
using Spinx.Data.Infrastructure;
using Spinx.Domain.AdminRolePermissions;
using Spinx.Domain.AdminUsers;
using Spinx.Domain.ContactUsInquiries;
using Spinx.Domain.EmailTemplates;
using Spinx.Domain.Members;
using Spinx.Domain.Pages;
using Spinx.Domain.QuizAnswers;
using Spinx.Domain.QuizCategories;
using Spinx.Domain.QuizQuestions;
using Spinx.Domain.Quizs;
using Spinx.Domain.SeoPages;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Spinx.Data.Configuration.GeneralSettings;
using Spinx.Domain.GeneralSettings;

namespace Spinx.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext() : base("DefaultConnectionString")
        {
            // Database.SetInitializer<SqlContext>(null);
        }

        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<AdminRole> AdminRoles { get; set; }
        public DbSet<AdminPermission> AdminPermissions { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<QuizCategory> QuizCategories { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizAnswer> QuizAnswers { get; set; }
        public DbSet<Quiz> Quizs { get; set; }

        public DbSet<ContactUsInquiry> ContactUsInquiry { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<MemberQuizAnswer> MemberQuizAnswer { get; set; }
        public DbSet<MemberResult> MemberResult { get; set; }

        public DbSet<SeoPage> SeoPage { get; set; }
        public DbSet<GeneralSetting> GeneralSetting { get; set; }

        public virtual void Commit()
        {
            SaveChanges();
        }

        public override int SaveChanges()
        {
            foreach (var history in ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .Select(e => e.Entity as IModificationHistory))
            {
                if (history == null) continue;
                history.UpdatedAt = DateTime.Now;
                if (history.CreatedAt == DateTime.MinValue)
                    history.CreatedAt = DateTime.Now;
            }

            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var newException = new FormattedDbEntityValidationException(e);
                throw newException;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AdminUserConfiguration());
            modelBuilder.Configurations.Add(new AdminRoleConfiguration());
            modelBuilder.Configurations.Add(new AdminPermissionConfiguration());
            modelBuilder.Configurations.Add(new PageConfiguration());
            modelBuilder.Configurations.Add(new EmailTemplateConfiguration());
            modelBuilder.Configurations.Add(new QuizCategoryConfiguration());
            modelBuilder.Configurations.Add(new QuizConfiguration());
            modelBuilder.Configurations.Add(new QuizQuestionConfiguration());
            modelBuilder.Configurations.Add(new QuizAnswerConfiguration());
            modelBuilder.Configurations.Add(new ContactUsInquiryConfiguration());
            modelBuilder.Configurations.Add(new MemberConfiguration());
            modelBuilder.Configurations.Add(new MemberQuizAnswerConfiguration());
            modelBuilder.Configurations.Add(new MemberResultConfiguration());

            modelBuilder.Configurations.Add(new SeoPageConfiguration());
            modelBuilder.Configurations.Add(new GeneralSettingConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}