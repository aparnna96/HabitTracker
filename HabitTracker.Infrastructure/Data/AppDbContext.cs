using HabitTracker.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {
        }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitLog> HabitLogs { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);
                //habit  entity configuration 
                modelBuilder.Entity<Habit>(entity =>
                {
                    entity.ToTable("Habits");
                    entity.HasKey(h => h.Id);
                    entity.Property(h => h.Name)
                        .IsRequired()
                        .HasMaxLength(100);

                    entity.Property(h => h.Description)
                    .HasMaxLength(500);

                    entity.Property(h => h.Frequency)
                        .IsRequired()
                        .HasConversion<int>();

                    entity.Property(h => h.CreatedAt)
                         .IsRequired()
                         .HasDefaultValueSql("GETUTCDATE()");

                    entity.HasIndex(h => h.UserId)
                         .HasDatabaseName("IX_Habits_UserId");

                    entity.HasIndex(h => h.CategoryId)
                         .HasDatabaseName("IX_Habits_CategoryId");

                    entity.HasIndex(h => h.IsActive)
                        .HasDatabaseName("IX_Habits_IsActive");

                    entity.HasOne(h => h.User)
                        .WithMany(u => u.Habits)
                        .HasForeignKey(h => h.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasOne(h => h.Category)
                       .WithMany(c => c.Habits)
                       .HasForeignKey(h => h.CategoryId)
                       .OnDelete(DeleteBehavior.SetNull);
                });

                modelBuilder.Entity<HabitLog>(entity =>
                {
                    entity.ToTable("HabitLogs");

                    entity.HasKey(hl => hl.Id);

                    entity.Property(hl => hl.CompletedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

                    entity.Property(hl => hl.CompletionDate)
                .IsRequired()
                .HasColumnType("date");

                    entity.Property(hl => hl.Notes)
                .HasMaxLength(500);

                    // Indexes
                    entity.HasIndex(hl => hl.HabitId)
                .HasDatabaseName("IX_HabitLogs_HabitId");

                    entity.HasIndex(hl => hl.CompletionDate)
                .HasDatabaseName("IX_HabitLogs_CompletionDate");

                    // Composite index for unique constraint
                    entity.HasIndex(hl => new { hl.HabitId, hl.CompletionDate })
                .IsUnique()
                .HasDatabaseName("IX_HabitLogs_HabitId_CompletionDate");

                    // Relationship
                    entity.HasOne(hl => hl.Habit)
                .WithMany(h => h.HabitLogs)
                .HasForeignKey(hl => hl.HabitId)
                .OnDelete(DeleteBehavior.Cascade); // Delete logs when habit is deleted
                });

                // ========================================
                // CATEGORY ENTITY CONFIGURATION
                // ========================================

                modelBuilder.Entity<Category>(entity =>
                {
                    entity.ToTable("Categories");

                    entity.HasKey(c => c.Id);

                    entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

                    entity.Property(c => c.Description)
                .HasMaxLength(200);

                    entity.Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

                    // Indexes
                    entity.HasIndex(c => c.UserId)
                .HasDatabaseName("IX_Categories_UserId");

                    // Composite index to prevent duplicate category names per user
                    entity.HasIndex(c => new { c.UserId, c.Name })
                .IsUnique()
                .HasDatabaseName("IX_Categories_UserId_Name");

                    // Relationship
                    entity.HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete categories when user is deleted

                });
        }
    }
}
