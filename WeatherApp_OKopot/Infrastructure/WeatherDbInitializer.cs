using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WeatherApp_OKopot.Entities;

namespace WeatherApp_OKopot.Infrastructure
{
    public class WeatherDbInitializer:DropCreateDatabaseIfModelChanges<WeatherDBContext>
    {
        protected override void Seed(WeatherDBContext context)
        {
            context.CityEntities.AddRange(
                new List<CityEntity>
                {
                    new CityEntity
                    {
                        Name = "Киев",
                        AddToMainList = true
                    },
                    new CityEntity
                    {
                        Name = "Львов",
                        AddToMainList = true
                    },
                    new CityEntity
                    {
                        Name = "Днепропетровск",
                        AddToMainList = true
                    },
                    new CityEntity
                    {
                        Name = "Харьков",
                        AddToMainList = true
                    },
                    new CityEntity
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