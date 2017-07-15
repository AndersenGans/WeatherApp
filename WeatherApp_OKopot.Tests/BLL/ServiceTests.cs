using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using WeatherApp_OKopot.BLL.BusinessModels;
using WeatherApp_OKopot.BLL.Infrastructure;
using WeatherApp_OKopot.BLL.Services;
using WeatherApp_OKopot.DAL.Entities;
using WeatherApp_OKopot.DAL.Interfaces;
using WeatherApp_OKopot.Infrastructure;

namespace WeatherApp_OKopot.Tests.BLL
{
    [TestFixture]
    public class ServiceTests
    {
        private Service servise;
        Mock<IUnitOfWork> mockUOF; 


        [SetUp]
        public void Init()
        {
            mockUOF = new Mock<IUnitOfWork>();

            var cities = GetStubForCities();
            var weathers = GetStubForWeathers();
            var histories = GetStubForHistories();

            mockUOF.Setup(a => a.Cities.GetAll()).Returns(cities);

            mockUOF.Setup(a => a.Weathers.GetAll()).Returns(weathers);

            mockUOF.Setup(a => a.Histories.GetAll()).Returns(histories);

            servise = new Service(mockUOF.Object);

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MapperWebProfile());
                cfg.AddProfile(new MapperBLLProfile());
            });
        }

        [Test]
        public async Task RequestingFromAPI_When_GiveCityName_and_CountOfDays_and_Key_Then_ReturnsCorrectResponce()
        {
            //Arrange
            string cityName = "Харьков";
            int countOfDays = 1;
            string key = "a0e092f89087b55a736a66d0519de7cb";
            
            //Act
            var rootObj = await servise.RequestingFromAPI(cityName, countOfDays, key);

            //Assert
            Assert.IsNotNull(rootObj);
        }

        [Test]
        public void CheckingCity_When_GiveCityNameWhichIsInTheDB_and_OtherCorrectParametrs_Then_ReturnsCorrectInstanceOfCity()
        {
            //Arrange
            string cityName = "Харьков";
            bool addToList = true;
            var rootObj = new RootObject()
            {
                city = new CityAPI()
                {
                    name = "Kharkiv"
                }
            };

            //Act
            var city = servise.CheckingCity(cityName, rootObj, addToList);

            //Assert
            Assert.That(cityName == city.Name && "Kharkiv" == city.AlternativeName && city.AddToMainList);
            mockUOF.Verify(a => a.Cities.Find(It.IsAny<Func<City, bool>>()), Times.AtLeastOnce);
            mockUOF.Verify(a => a.Cities.Create(It.IsAny<City>()), Times.AtLeastOnce);
            mockUOF.Verify(a => a.Save(), Times.AtLeastOnce);
        }

        [Test]
        public void CheckingCity_When_GiveCityNameWhichIsNotInTheDB_and_OtherCorrectParametrs_Then_ReturnsCorrectInstanceOfCity()
        {
            //Arrange
            string cityName = "Харьков";
            bool addToList = true;
            var rootObj = new RootObject()
            {
                city = new CityAPI()
                {
                    name = "Kharkiv"
                }
            };

            //Act
            var city = servise.CheckingCity(cityName, rootObj, addToList);

            //Assert
            Assert.That(cityName == city.Name && "Kharkiv" == city.AlternativeName && city.AddToMainList);
        }

        [Test]
        public void FindWeathers_When_GiveCityNameCorrect_Then_ReturnsCollectionOfWeathers()
        {
            //Arrange
            var cityName = "Харьков";
            mockUOF.Setup(s => s.Weathers.Find(It.IsAny<Func<Weather, bool>>()))
                .Returns(new List<Weather>(){ new Weather() });
            
            //Act
            var collectionWeathers = servise.FindWeathers(cityName);

            //Assert
            Assert.IsNotEmpty(collectionWeathers);
        }

        [Test]
        public void DeleteCitiesFromMainList_When_GiveCorrectCityName_Then_CallMethodsUpdate_and_Save()
        {
            //Arrange
            var cityName = "Харьков";

            //Act
            servise.DeleteCitiesFromMainList(cityName);

            //Assert
            mockUOF.Verify(a => a.Cities.Update(It.IsAny<City>()), Times.AtLeastOnce);
            mockUOF.Verify(a => a.Save(), Times.AtLeastOnce);
        }

       [Test]
        public void GetCityByName_When_GiveCityNameCorrect_Then_ReturnsInstanceCityDTO()
        {
            //Arrange
            var cityName = "Харьков";

            //Act
            var city = servise.GetCityByName(cityName);

            //Assert
            Assert.AreEqual(city.Name, cityName);
        }

        [Test]
        public async Task GetDailyWeathers_When_GiveCorrectParams_Then_DailyWeatherSavesToDB()
        {
            //Arrange
            string cityName = "Харьков";
            bool addToList = true;
            string key = "a0e092f89087b55a736a66d0519de7cb";

            //Act
            await servise.GetDailyWeathers(cityName, key, addToList);

            //Assert
            mockUOF.Verify(a => a.Weathers.Find(It.IsAny<Func<Weather, bool>>()), Times.AtLeastOnce);
            mockUOF.Verify(a => a.Weathers.Create(It.IsAny<Weather>()), Times.AtLeastOnce);
            mockUOF.Verify(a => a.Histories.Create(It.IsAny<History>()), Times.AtLeastOnce);
            mockUOF.Verify(a => a.Save(), Times.AtLeastOnce);
        }

        [Test]
        public async Task GetManyDaysWeathers_When_GiveCorrectParams_Then_DailyWeatherSavesToDB()
        {
            //Arrange
            string cityName = "Харьков";
            int countDays = 3;
            string key = "a0e092f89087b55a736a66d0519de7cb";

            //Act
            await servise.GetManyDaysWeathers(cityName, key, countDays);

            //Assert
            mockUOF.Verify(a => a.Weathers.Find(It.IsAny<Func<Weather, bool>>()), Times.AtLeastOnce);
            mockUOF.Verify(a => a.Weathers.Create(It.IsAny<Weather>()), Times.AtLeastOnce);
            mockUOF.Verify(a => a.Save(), Times.AtLeastOnce);
        }

        private List<City> GetStubForCities() {
            List<City> cities = new List<City>
            {
                new City
                {
                    CityId = 1, AddToMainList = false, Name = "Харьков", AlternativeName = ""
                },
                new City
                {
                    CityId = 2, AddToMainList = false, Name = "Киев", AlternativeName = ""
                },
                new City
                {
                    CityId = 3, AddToMainList = false, Name = "Львов", AlternativeName = ""
                },
            };

            return cities;
        }

        private List<Weather> GetStubForWeathers()
        {
            List<Weather> weathers = new List<Weather>
            {
                new Weather
                {
                    CityId = 1, WeatherId = 1, City = new City{ CityId = 1, Name = "Харьков"}
                },
                new Weather
                {
                    CityId = 1, WeatherId = 2, City = new City{ CityId = 1, Name = "Харьков"}
                },
                new Weather
                {
                    CityId = 2, WeatherId = 3, City = new City{ CityId = 2, Name = "Киев"}
                },
                new Weather
                {
                    CityId = 2, WeatherId = 4, City = new City{ CityId = 2, Name = "Киев"}
                },
                new Weather
                {
                    CityId = 3, WeatherId = 3, City = new City{ CityId = 3, Name = "Львов"}
                },
                new Weather
                {
                    CityId = 3, WeatherId = 4, City = new City{ CityId = 3, Name = "Львов"}
                }
            };
            return weathers;
        }

        private List<History> GetStubForHistories()
        {
            List<History> histories = new List<History>
            {
                new History
                {
                    CityId = 1, Id = 1
                },
                new History
                {
                    CityId = 2, Id = 2
                },
                new History
                {
                    CityId = 3, Id = 3
                }
            };
            return histories;
        }
    }
}