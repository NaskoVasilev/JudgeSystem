using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Models;
using JudgeSystem.Data.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JudgeSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

		public DbSet<Course> Courses { get; set; }
		public DbSet<Lesson> Lessons { get; set; }
		public DbSet<Problem> Problems { get; set; }
		public DbSet<Resource> Resources { get; set; }
		public DbSet<Test> Tests { get; set; }
		public DbSet<ExecutedTest> ExecutedTests { get; set; }
		public DbSet<Contest> Contests { get; set; }
		public DbSet<UserContest> UserContests { get; set; }
		public DbSet<Submission> Submissions { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Practice> Practices { get; set; }
        public DbSet<UserPractice> UserPractices { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<AllowedIpAddress> AllowedIpAddresses { get; set; }
        public DbSet<AllowedIpAddressContest> AllowedIpAddressContests { get; set; }

        public override int SaveChanges() => SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AllowedIpAddressContest>(entity =>
            {
                entity.HasKey(e => new { e.AllowedIpAddressId, e.ContestId });
            });

            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            ConfigureUserIdentityRelations(builder);

            ConfigureApplicationEntitiesRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            IEnumerable<IMutableEntityType> deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (IMutableEntityType deletableEntityType in deletableEntityTypes)
            {
                MethodInfo method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            IEnumerable<IMutableForeignKey> foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (IMutableForeignKey foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private void ConfigureApplicationEntitiesRelations(ModelBuilder builder)
        {
            builder.Entity<ExecutedTest>()
                .HasOne(t => t.Submission)
                .WithMany(s => s.ExecutedTests)
                .HasForeignKey(t => t.SubmissionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ExecutedTest>()
                .HasOne(et => et.Test)
                .WithMany(t => t.ExecutedTests)
                .HasForeignKey(et => et.TestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Contest>()
                .HasMany(c => c.UserContests)
                .WithOne(uc => uc.Contest)
                .HasForeignKey(uc => uc.ContestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserContest>().HasKey(uc => new { uc.UserId, uc.ContestId });

            builder.Entity<Submission>().HasOne(s => s.Contest)
                .WithMany(c => c.Submissions)
                .HasForeignKey(s => s.ContestId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserPractice>()
               .HasKey(x => new { x.PracticeId, x.UserId });

            builder.Entity<Practice>()
                .HasOne(x => x.Lesson)
                .WithOne(x => x.Practice)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureUserIdentityRelations(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

			builder.Entity<ApplicationUser>()
				.HasMany(u => u.UserContests)
				.WithOne(uc => uc.User)
				.HasForeignKey(uc => uc.UserId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<ApplicationUser>()
				.HasOne(u => u.Student)
				.WithOne(s => s.User)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);
		}

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity => builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);

        private void ApplyAuditInfoRules()
        {
            IEnumerable<EntityEntry> changedEntries = ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (EntityEntry entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
