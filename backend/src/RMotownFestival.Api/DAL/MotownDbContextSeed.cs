using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMotownFestival.Api.Domain;
using RMotownFestival.Api.Data;

namespace RMotownFestival.Api.DAL
{
    public class MotownDbContextSeed
    {
        public void Seed(ModelBuilder modelBuilder)
        {
            /*
            foreach(var stage in FestivalDataSource.Current.Stages)
            {
                modelBuilder.Entity<Stage>().HasData(stage);
            }
            */

            modelBuilder.Entity<Stage>().HasData(FestivalDataSource.Current.Stages);
            modelBuilder.Entity<Artist>().HasData(FestivalDataSource.Current.Artists);
            //modelBuilder.Entity<ScheduleItem>().HasData(FestivalDataSource.Current.LineUp.Items);
            //modelBuilder.Entity<Schedule>().HasData(FestivalDataSource.Current.LineUp);

        }
    }
}
