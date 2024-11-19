using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace CloudQuick.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.StudentName).IsRequired();
            builder.Property(n => n.StudentName).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Address).IsRequired(false) .HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);

            builder.HasData(new List<Student>
            {
                new Student {
                    Id = 1,
                    StudentName = "Bilal Shah",
                    Address = "Pakistan",
                    Email = "Shah@gmail.com",
                    DOB = new DateTime(2024, 11, 11)
                },
                new Student {
                    Id = 2,
                    StudentName = "Bilal",
                    Address = "Pakistan",
                    Email = "Bilal@gmail.com",
                    DOB = new DateTime(2024, 11, 14)
                }
            });
        }
    }
}
