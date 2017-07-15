using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NUnit.Framework;
using WeatherApp_OKopot.BLL.BusinessModels;
using WeatherApp_OKopot.BLL.Infrastructure;
using WeatherApp_OKopot.BLL.Services;
using WeatherApp_OKopot.DAL.Interfaces;
using WeatherApp_OKopot.DAL.Repositories;

namespace WeatherApp_OKopot.IntegrationTests.BLL
{
    [TestFixture]
    public class IntegrationServiceTests
    {
        private IUnitOfWork uow;
        private Service service;
        private string key;

        [SetUp]
        public void Init()
        {
            uow = new EFUnitOfWork("FakeDB");
            service = new Service(uow);
            key = "a0e092f89087b55a736a66d0519de7cb";
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MapperBLLProfile());
            });
        }

        [Test]
        public void FindCitiesToAddToMainList_When_NeedsCities_Then_ReturnsCities()
        {
            //Act
            var result = service.FindCitiesToAddToMainList();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.All(a => a.AddToMainList));
        }

        [Test]
        public async Task GetDailyWeathers_When_GiveCorrectParametrs_Then_SavesWeathersAndHistoriesToDB()
        {
            //Arrange
            var cityName = "Киев";
            var addToList = true;

            //Act
            await service.GetDailyWeathers(cityName, key, addToList);
            var result = uow.Weathers.GetAll();

            //Assert
            Assert.IsTrue(result.Any(a => a.City.Name == cityName));
        }

        [Test]
        public async Task GetManyDaysWeathers_When_GiveCorrectParametrs_Then_SavesWeathersAndHistoriesToDB()
        {
            //Arrange
            var cityName = "Киев";
            var countOfDays = 7;

            //Act
            await service.GetManyDaysWeathers(cityName, key, countOfDays);
            var result = uow.Weathers.GetAll();

            //Assert
            Assert.IsTrue(result.Any(a => a.City.Name == cityName));
        }

        [Test]
        public async Task FindWeathers_When_GiveCorrectParametrs_Then_ReturnsCollectionOfWeathers()
        {
            //Arrange
            var cityName = "Киев";
            var countOfDays = 7;
            //Act
            await service.GetManyDaysWeathers(cityName, key, countOfDays);
            var result = service.FindWeathers(cityName);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result.Count() == countOfDays);
        }

        [Test]
        public async Task GetHistories_When_GiveCorrectParametrs_Then_ReturnsCollectionOfHistories()
        {
            //Arrange
            var cityName = "Киев";
            var countOfDays = 7;

            //Act
            await service.GetManyDaysWeathers(cityName, key, countOfDays);
            var result = service.GetHistories();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task DeleteCitiesFromMainList_When_GiveCorrectCityName_Then_CityProperty_addToMainList_ChangeToFalse()
        {
            //Arrange
            var cityName = "Киев";
            var addToList = true;

            //Act
            await service.GetDailyWeathers(cityName, key, addToList);
            service.DeleteCitiesFromMainList(cityName);
            var result = service.FindCitiesToAddToMainList();

            //Assert
            Assert.That(result.All(a => a.Name != cityName));
        }

        [Test]
        public void GetCityByName_When_GiveCorrectCityName_Then_ReturnInstanseOfCityDTO()
        {
            //Arrange
            var cityName = "Киев";

            //Act
            var result = service.GetCityByName(cityName);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, cityName);
        }

        [Test]
        public void DeleteHistories_When_NeedsToDeleteAllHistories_Then_DeleteThem()
        {
            //Act
            service.DeleteHistories();
            var result = service.GetHistories();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void CheckingCity_When_GiveCorrectParametrs_Then_ReturnInstanseOfCity()
        {
            //Arrange
            var cityName = "Харьков";
            var rootObj = new RootObject()
            {
                city = new CityAPI()
                {
                    name = "Kharkiv"
                }
            };
            var addToList = true;

            //Act
            service.CheckingCity(cityName, rootObj, addToList);
            var result = service.GetCityByName(cityName);

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.AlternativeName == rootObj.city.name && result.AddToMainList);
        }

        [Test]
        public async Task RequestingFromAPI_When_GiveCorrectParametrs_Then_RetirnInstanseOfRootObj()
        {
            //Act
            string cityName = "Харьков";
            int countOfDays = 1;

            //Act
            var rootObj = service.RequestingFromAPI(cityName, countOfDays, key);

            //Assert
            Assert.IsNotNull(rootObj);
        }
    }
}