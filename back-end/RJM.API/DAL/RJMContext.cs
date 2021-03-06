using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RJM.API.Models;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace RJM.API.DAL
{
    public class RJMContext : IdentityDbContext<
        User,
        IdentityRole<Guid>,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>
    >
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RJMContext(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration
        ) : base()
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        // General
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentContent> DocumentContents { get; set; }
        public DbSet<DocumentResume> DocumentResume { get; set; }
        public DbSet<DocumentSkill> DocumentSkill { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ResumeState> ResumeStates { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillAlias> SkillAliases { get; set; }
        public DbSet<ResumeSkill> ResumeSkill { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobState> JobStates { get; set; }
        public DbSet<JobSkill> JobSkill { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("RJMContext"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Identity

            modelBuilder.Entity<User>(e => e.ToTable("Users"));
            modelBuilder.Entity<IdentityRole<Guid>>(e => e.ToTable("Roles"));
            modelBuilder.Entity<IdentityUserRole<Guid>>(e =>
            {
                e.ToTable("UserRoles");
                // In case you changed the TKey type
                e.HasKey(key => new { key.UserId, key.RoleId });
            });
            modelBuilder.Entity<IdentityUserClaim<Guid>>(e => e.ToTable("UserClaims"));
            modelBuilder.Entity<IdentityUserLogin<Guid>>(e =>
            {
                e.ToTable("UserLogins");
                // In case you changed the TKey type
                e.HasKey(key => new { key.ProviderKey, key.LoginProvider });
            });
            modelBuilder.Entity<IdentityRoleClaim<Guid>>(e => e.ToTable("RoleClaims"));
            modelBuilder.Entity<IdentityUserToken<Guid>>(e =>
            {
                e.ToTable("UserTokens");
                // In case you changed the TKey type
                e.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
            });

            #endregion

            #region Documents

            // Soft delete query filter
            modelBuilder.Entity<Document>()
                .HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<Document>()
                .ToTable("Documents");

            // Key
            modelBuilder.Entity<Document>()
                .HasKey(e => e.Id);

            // Properties
            modelBuilder.Entity<Document>()
                .Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<Document>()
                .Property(e => e.DisplayName)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<Document>()
                .Property(e => e.Description)
                .HasMaxLength(512);

            modelBuilder.Entity<Document>()
                .Property(e => e.MimeType)
                .HasMaxLength(100);

            // User
            modelBuilder.Entity<Document>()
                .HasOne(x => x.User)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Document>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Document>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region DocumentTypes

            // Soft delete query filter
            modelBuilder.Entity<DocumentType>()
                .HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<DocumentType>()
                .ToTable("DocumentTypes");

            // Key
            modelBuilder.Entity<DocumentType>()
                .HasKey(e => e.Id);

            // Properties
            modelBuilder.Entity<DocumentType>()
                .Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<DocumentType>()
                .Property(e => e.DisplayName)
                .HasMaxLength(256)
                .IsRequired();

            // User
            modelBuilder.Entity<DocumentType>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DocumentType>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region DocumentContents

            // Soft delete query filter
            modelBuilder.Entity<DocumentContent>()
                .HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<DocumentContent>()
                .ToTable("DocumentContents");

            // Key
            modelBuilder.Entity<DocumentContent>()
                .HasKey(e => e.Id);

            // Properties
            modelBuilder.Entity<DocumentContent>()
                .Property(e => e.Text)
                .IsRequired();

            modelBuilder.Entity<DocumentContent>()
                .Property(e => e.TextType)
                .IsRequired();

            #endregion

            #region DocumentResume

            // Soft delete query filter
            modelBuilder.Entity<DocumentResume>().HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<DocumentResume>().ToTable("DocumentResume");

            // Key
            modelBuilder.Entity<DocumentResume>().HasKey(e => e.Id);

            // Required properties
            modelBuilder.Entity<DocumentResume>().Property(e => e.DocumentId).IsRequired();
            modelBuilder.Entity<DocumentResume>().Property(e => e.ResumeId).IsRequired();

            // User
            modelBuilder.Entity<DocumentResume>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DocumentResume>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region DocumentSkill

            // Soft delete query filter
            modelBuilder.Entity<DocumentSkill>().HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<DocumentSkill>().ToTable("DocumentSkill");

            // Key
            modelBuilder.Entity<DocumentSkill>().HasKey(e => e.Id);

            // Required properties
            modelBuilder.Entity<DocumentSkill>().Property(e => e.DocumentId).IsRequired();
            modelBuilder.Entity<DocumentSkill>().Property(e => e.SkillId).IsRequired();

            // User
            modelBuilder.Entity<DocumentSkill>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DocumentSkill>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Resumes

            // Soft delete query filter
            modelBuilder.Entity<Resume>()
                .HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<Resume>()
                .ToTable("Resumes");

            // Key
            modelBuilder.Entity<Resume>()
                .HasKey(e => e.Id);

            // Properties
            modelBuilder.Entity<Resume>()
                .Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();
            modelBuilder.Entity<Resume>()
                .Property(e => e.DisplayName)
                .HasMaxLength(256)
                .IsRequired();
            modelBuilder.Entity<Resume>()
                .Property(e => e.ResumeStateId)
                .IsRequired();

            // User
            modelBuilder.Entity<Resume>()
                .HasOne(x => x.User)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Resume>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Resume>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region ResumeStates

            // Soft delete query filter
            modelBuilder.Entity<ResumeState>().HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<ResumeState>().ToTable("ResumeStates");

            // Key
            modelBuilder.Entity<ResumeState>().HasKey(e => e.Id);

            // Required properties
            modelBuilder.Entity<ResumeState>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<ResumeState>().Property(e => e.DisplayName).IsRequired();

            // User
            modelBuilder.Entity<ResumeState>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ResumeState>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Skills

            // Soft delete query filter
            modelBuilder.Entity<Skill>().HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<Skill>().ToTable("Skills");

            // Key
            modelBuilder.Entity<Skill>().HasKey(e => e.Id);

            // Properties
            modelBuilder.Entity<Skill>()
                .Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<Skill>()
                .Property(e => e.DisplayName)
                .HasMaxLength(256)
                .IsRequired();

            // User
            modelBuilder.Entity<Skill>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Skill>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region SkillAliases

            // Soft delete query filter
            modelBuilder.Entity<SkillAlias>().HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<SkillAlias>().ToTable("SkillAliases");

            // Key
            modelBuilder.Entity<SkillAlias>().HasKey(e => e.Id);

            // Required properties
            modelBuilder.Entity<SkillAlias>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<SkillAlias>().Property(e => e.SkillId).IsRequired();

            // User
            modelBuilder.Entity<SkillAlias>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SkillAlias>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region ResumeSkill

            // Soft delete query filter
            modelBuilder.Entity<ResumeSkill>().HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<ResumeSkill>().ToTable("ResumeSkill");

            // Key
            modelBuilder.Entity<ResumeSkill>().HasKey(e => e.Id);

            // Required properties
            modelBuilder.Entity<ResumeSkill>().Property(e => e.ResumeId).IsRequired();
            modelBuilder.Entity<ResumeSkill>().Property(e => e.SkillId).IsRequired();

            // User
            modelBuilder.Entity<ResumeSkill>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ResumeSkill>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Jobs

            // Soft delete query filter
            modelBuilder.Entity<Job>().HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<Job>().ToTable("Jobs");

            // Key
            modelBuilder.Entity<Job>().HasKey(e => e.Id);

            // Required properties
            modelBuilder.Entity<Job>().Property(e => e.JobStateId).IsRequired();

            // User
            modelBuilder.Entity<Job>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Job>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region JobStates

            // Soft delete query filter
            modelBuilder.Entity<JobState>().HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<JobState>().ToTable("JobStates");

            // Key
            modelBuilder.Entity<JobState>().HasKey(e => e.Id);

            // Required properties
            modelBuilder.Entity<JobState>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<JobState>().Property(e => e.DisplayName).IsRequired();

            // User
            modelBuilder.Entity<JobState>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobState>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region JobSkill

            // Soft delete query filter
            modelBuilder.Entity<JobSkill>().HasQueryFilter(e => e.DeletedOn == null);

            // Table
            modelBuilder.Entity<JobSkill>().ToTable("JobSkill");

            // Key
            modelBuilder.Entity<JobSkill>().HasKey(e => e.Id);

            // Required properties
            modelBuilder.Entity<JobSkill>().Property(e => e.JobId).IsRequired();
            modelBuilder.Entity<JobSkill>().Property(e => e.SkillId).IsRequired();

            // User
            modelBuilder.Entity<JobSkill>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobSkill>()
                .HasOne(x => x.ModifiedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
        }

        public override int SaveChanges()
        {
            SoftDeleteLogic();
            TimestampsLogic();
            UserInfoDataLogic();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            SoftDeleteLogic();
            TimestampsLogic();
            UserInfoDataLogic();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void SoftDeleteLogic()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                // Models that have soft delete
                if (
                    entry.Entity.GetType() == typeof(Document) ||
                    entry.Entity.GetType() == typeof(DocumentType) ||
                    entry.Entity.GetType() == typeof(DocumentContent) ||
                    entry.Entity.GetType() == typeof(DocumentResume) ||
                    entry.Entity.GetType() == typeof(DocumentSkill) ||
                    entry.Entity.GetType() == typeof(Resume) ||
                    entry.Entity.GetType() == typeof(ResumeState) ||
                    entry.Entity.GetType() == typeof(Skill) ||
                    entry.Entity.GetType() == typeof(SkillAlias) ||
                    entry.Entity.GetType() == typeof(ResumeSkill) ||
                    entry.Entity.GetType() == typeof(Job) ||
                    entry.Entity.GetType() == typeof(JobState) ||
                    entry.Entity.GetType() == typeof(JobSkill)
                )
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["DeletedOn"] = null;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["DeletedOn"] = DateTime.Now;
                            break;
                    }
                }
            }
        }

        private void TimestampsLogic()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                // Models that have soft delete
                if (
                    entry.Entity.GetType() == typeof(Document) ||
                    entry.Entity.GetType() == typeof(DocumentType) ||
                    entry.Entity.GetType() == typeof(DocumentContent) ||
                    entry.Entity.GetType() == typeof(DocumentResume) ||
                    entry.Entity.GetType() == typeof(DocumentSkill) ||
                    entry.Entity.GetType() == typeof(Resume) ||
                    entry.Entity.GetType() == typeof(ResumeState) ||
                    entry.Entity.GetType() == typeof(Skill) ||
                    entry.Entity.GetType() == typeof(SkillAlias) ||
                    entry.Entity.GetType() == typeof(ResumeSkill) ||
                    entry.Entity.GetType() == typeof(Job) ||
                    entry.Entity.GetType() == typeof(JobState) ||
                    entry.Entity.GetType() == typeof(JobSkill)
                )
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["CreatedOn"] = DateTime.Now;
                            entry.CurrentValues["ModifiedOn"] = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            entry.CurrentValues["ModifiedOn"] = DateTime.Now;
                            break;
                    }
                }
            }
        }

        private void UserInfoDataLogic()
        {
            string userIdString = this.httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdString))
            {
                Guid userId = Guid.Parse(userIdString);

                foreach (var entry in ChangeTracker.Entries())
                {
                    Type entityType = entry.Entity.GetType();
                    if (
                        entry.Entity.GetType() == typeof(Document) ||
                        entry.Entity.GetType() == typeof(DocumentType) ||
                        //entry.Entity.GetType() == typeof(DocumentContent) ||
                        entry.Entity.GetType() == typeof(DocumentResume) ||
                        entry.Entity.GetType() == typeof(DocumentSkill) ||
                        entry.Entity.GetType() == typeof(Resume) ||
                        entry.Entity.GetType() == typeof(ResumeState) ||
                        entry.Entity.GetType() == typeof(Skill) ||
                        entry.Entity.GetType() == typeof(SkillAlias) ||
                        entry.Entity.GetType() == typeof(ResumeSkill) ||
                        entry.Entity.GetType() == typeof(Job) ||
                        entry.Entity.GetType() == typeof(JobState) ||
                        entry.Entity.GetType() == typeof(JobSkill)
                    )
                    {
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.CurrentValues["CreatedByUserId"] = userId;
                                entry.CurrentValues["ModifiedByUserId"] = userId;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues["ModifiedByUserId"] = userId;
                                break;
                        }
                    }
                }
            }
        }
    }
}
