using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Services.DBModel
{
    public class JobsContext : DbContext
    {
        public JobsContext([NotNull] DbContextOptions<JobsContext> options) : base(options) { }

        public DbSet<Job> Jobs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>().HasData(
                new Job { JobId = 1, Name = "Job 1", },
                new Job { JobId = 2, Name = "Job 2", }, 
                new Job { JobId = 3, Name = "Job 3", });
        }
    }

    public class Job
    {
        public int JobId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
