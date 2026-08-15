using Domain.Models.Identity;
using Domain.Models.Lessons;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PasswordResetOtp> PasswordResetOtps { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grades");
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).IsRequired().HasMaxLength(100);

                entity.HasMany(g => g.Students)
                      .WithOne(u => u.Grade)
                      .HasForeignKey(u => u.GradeId)
                      .OnDelete(DeleteBehavior.Restrict);

            });
            builder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");
                entity.Property(rt => rt.Token)
                      .IsRequired()
                      .HasMaxLength(256);
                entity.HasIndex(rt => rt.Token)
                      .IsUnique();
                entity.HasOne(rt => rt.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(rt => rt.Token).IsRequired();
            });

            builder.Entity<PasswordResetOtp>(entity =>
            {
                entity.ToTable("PasswordResetOtps");

                entity.Property(o => o.Code)
                      .IsRequired()
                      .HasMaxLength(6);

                entity.HasOne(o => o.User)
                      .WithMany()
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
