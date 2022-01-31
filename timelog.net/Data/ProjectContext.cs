using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using timelog.net.Models;

namespace timelog.net
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects => Set<Project>();

        public DbSet<Models.Task> Tasks => Set<Models.Task>();

        public DbSet<Entry> Entries => Set<Entry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Models.Task>().ToTable("Task");
            modelBuilder.Entity<Entry>().ToTable("Entry");
        }
    }
}