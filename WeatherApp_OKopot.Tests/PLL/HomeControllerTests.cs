using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Moq;
using NUnit.Framework;
using WeatherApp_OKopot.BLL.DTO;
using WeatherApp_OKopot.BLL.Infrastructure;
using WeatherApp_OKopot.BLL.Interfaces;
using WeatherApp_OKopot.Controllers;
using WeatherApp_OKopot.Infrastructure;

namespace WeatherApp_OKopot.Tests.PLL
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IService> mockService;
        private HomeController homeController;

        [SetUp]
        public void Init()
        {
            mockService = new Mock<IService>();

            mockService.Setup(a => a.FindCitiesToAddToMainList()).Returns(new List<CityDTO>
            {
                new CityDTO
                {
                    AddToMainList = true
                }
            });

            mockService.Setup(a => a.FindWeathers(It.IsAny<string>()))
                .Returns(new List<WeatherDTO>()
                {
                    new WeatherDTO()
                });

            mockService.Setup(a => a.GetHistories()).Returns(new List<HistoryDTO>
            {
                new HistoryDTO()
            });

            mockService.Setup(a => a.GetCityByName(It.IsAny<string>()))
                .Returns(new CityDTO()
                {
                    AlternativeName = "SomeName"
                });

            homeController = new HomeController(mockService.Object);

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MapperWebProfile());
                cfg.AddProfile(new MapperBLLProfile());
            });
        }

        [Test]
        public async Task ShowDailyWeatherPartial_When_GiveCorrectParametrs_Then_ReturnCorrectResult()
        {
            //Arrange
            var stubCityName = "";

            //Act
            var result = await homeController.ShowDailyWeatherPartial(stubCityName) as PartialViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowDailyWeatherPartial", result.ViewName);
            mockService.Verify(a => a.GetDailyWeathers(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task ShowDailyWeather_When_GiveCorrectParametrs_Then_ReturnCorrectResult()
        {
            //Arrange
            var stubCityName = "City";

            //Act
            var result = await homeController.ShowDailyWeather(stubCityName, false) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            mockService.Verify(a => a.GetDailyWeathers(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task ShowThreeDaysWeather_When_GiveCorrectParametrs_Then_ReturnCorrectResult()
        {
            //Arrange
            var stubCityName = "City";

            //Act
            var result = await homeController.ShowThreeDaysWeather(stubCityName) as PartialViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowDailyWeatherPartial", result.ViewName);
            mockService.Verify(a => a.GetManyDaysWeathers(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    3),
                Times.AtLeastOnce);
        }

        [Test]
        public async Task ShowWeekWeather_When_GiveCorrectParametrs_Then_ReturnCorrectResult()
        {
            //Arrange
            var stubCityName = "City";

            //Act
            var result = await homeController.ShowWeekWeather(stubCityName) as PartialViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowDailyWeatherPartial", result.ViewName);
            mockService.Verify(a => a.GetManyDaysWeathers(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    7),
                Times.AtLeastOnce);
        }

        [Test]
        public void HistoryOfWeather_When_GiveCorrectParametrs_Then_ReturnCorrectResult()
        {
            //Act
            var result = homeController.HistoryOfWeather() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("HistoryOfWeather", result.ViewName);
            mockService.Verify(a => a.GetHistories(), Times.AtLeastOnce);
        }

        [Test]
        public void DeleteCity_When_GiveCorrectParametrs_Then_ReturnCorrectResult()
        {
            //Arrange
            var stubCityName = "City";

            //Act
            var result = homeController.DeleteCity(stubCityName) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            mockService.Verify(a => a.DeleteCitiesFromMainList(
                    It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void ClearHistory_When_GiveCorrectParametrs_Then_ReturnCorrectResult()
        {
            //Act
            var result = homeController.ClearHistory() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("HistoryOfWeather", result.ViewName);
            mockService.Verify(a => a.DeleteHistories(), Times.AtLeastOnce);
            mockService.Verify(a => a.GetHistories(), Times.AtLeastOnce);
        }

        [Test]
        public void Dispose_When_CallDispose_Then_CallServiceDispose()
        {
            //Act
            homeController.Dispose();

            //Assert
            mockService.Verify(a => a.Dispose(), Times.AtLeastOnce);
        }


    }
}