using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp_OKopot.DAL.Entities;

namespace WeatherApp_OKopot.DAL.EF
{
    class WeatherDbInitializer:DropCreateDatabaseIfModelChanges<WeatherDbContext>
    {
        protected override void Seed(WeatherDbContext context)
        {
            context.Cities.AddRange(
                new List<City>
                {
                    new City
                    {
                        Name = "Kyiv",
                        AddToMainList = true
                    },
                    new City
                    {
                        Name = "Lviv",
                        AddToMainList = true
                    },
                    new City
                    {
                        Name = "Dnipropetrovsk",
                        AddToMainList = true
                    },
                    new City
                    {
                        Name = "Kharkiv",
                        AddToMainList = true
                    },
                    new City
                    {
                        Name = "Odessa",
                        AddToMainList = true
                    }
                }
            );
            base.Seed(context);
        }
    }
}
