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
                        Name = "Киев",
                        AddToMainList = true
                    },
                    new City
                    {
                        Name = "Львов",
                        AddToMainList = true
                    },
                    new City
                    {
                        Name = "Днепропетровск",
                        AddToMainList = true
                    },
                    new City
                    {
                        Name = "Харьков",
                        AddToMainList = true
                    },
                    new City
                    {
                        Name = "Одесса",
                        AddToMainList = true
                    }
                }
            );
            base.Seed(context);
        }
    }
}
