using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using AutoMapper;
using Moq;
using NUnit.Framework;
using WeatherApp_OKopot.BLL.Infrastructure;
using WeatherApp_OKopot.BLL.Services;
using WeatherApp_OKopot.Controllers.APIControllers;
using WeatherApp_OKopot.DAL.Entities;
using WeatherApp_OKopot.DAL.Interfaces;
using WeatherApp_OKopot.Infrastructure;

namespace WeatherApp_OKopot.Tests.PLL
{
    public class WeathersControllerTests
    {
        private Mock<IUnitOfWork> mockUof;
        private Service service;
        private WeathersController weathersController;

        [SetUp]
        public void Init()
        {
            mockUof = new Mock<IUnitOfWork>();
            service = new Service(mockUof.Object);
            weathersController = new WeathersController(service);
            GetStubs();


            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MapperWebProfile());
                cfg.AddProfile(new MapperBLLProfile());
            });
        }

        [Test]
        public void Get_When_GiveCorrectParametrs_Then_NotReturnBadRequest()
        {
            //Arrange
            var cityName = "Kharkiv";
            var countOfDays = 3;

            //Act
            var result = weathersController.Get(cityName, countOfDays);

            //Assert
            Assert.IsNotInstanceOf<BadRequestErrorMessageResult>(result);
        }

        private void GetStubs()
        {
            mockUof.Setup(a => a.Cities.Find(It.IsAny<Func<City, bool>>())).Returns(new List<City>
            {
                new City()
            });

            mockUof.Setup(a => a.Weathers.Find(It.IsAny<Func<Weather, bool>>())).Returns(new List<Weather>
            {
                new Weather()
            });
        }
    }
}