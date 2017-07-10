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
                        Name = "Киев"
                    },
                    new CityEntity
                    {
                        Name = "Львов"
                    },
                    new CityEntity
                    {
                        Name = "Днепропетровск"
                    },
                    new CityEntity
                    {
                        Name = "Харьков"
                    },
                    new CityEntity
                    {
                        Name = "Одесса"
                    }
                }
            );
            base.Seed(context);
        }
    }
}