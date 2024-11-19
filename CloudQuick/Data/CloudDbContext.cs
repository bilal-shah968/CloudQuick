using CloudQuick.Data.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CloudQuick.Data
{
    public class CloudDbContext : DbContext
    {
        public CloudDbContext(DbContextOptions<CloudDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Table 1
            modelBuilder.ApplyConfiguration(new StudentConfig());
          
       
        }
    }
}
