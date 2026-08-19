using Domain.Models.Lessons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Title)
                   .IsRequired()
                   .HasMaxLength(150);
            builder.Property(l => l.Price)
                   .HasColumnType("decimal(18,2)");
            builder.HasQueryFilter(l => !l.IsDeleted);
            builder.HasOne(l => l.Grade)
                   .WithMany()
                   .HasForeignKey(l => l.GradeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
