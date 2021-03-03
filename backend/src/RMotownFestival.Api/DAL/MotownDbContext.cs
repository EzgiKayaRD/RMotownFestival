using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMotownFestival.Api.Domain;

namespace RMotownFestival.Api.DAL
{
    public class MotownDbContext : DbContext
    {
        public MotownDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Stage> Stage { get; set; }

        //public DbSet<Festival> Festivals { get; set; }

        //public DbSet<Schedule> Schedules { get; set; }

        public DbSet<ScheduleItem> ScheduleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new MotownDbContextSeed().Seed(modelBuilder);
        }
    }
}
