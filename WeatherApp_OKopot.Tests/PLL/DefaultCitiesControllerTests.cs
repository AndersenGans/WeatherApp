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
    [TestFixture]
    public class DefaultCitiesControllerTests
    {
        private Mock<IUnitOfWork> mockUof;
        private Service service;
        private DefaultCitiesController defaultCitiesController;

        [SetUp]
        public void Init()
        {
            mockUof = new Mock<IUnitOfWork>();
            service = new Service(mockUof.Object);
            defaultCitiesController = new DefaultCitiesController(service);
            GetStubs();


            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new MapperWebProfile());
                cfg.AddProfile(new MapperBLLProfile());
            });
        }

        [Test]
        public void Get_When_NeedsCitiesWhichAreInMainList_Then_NotReturnBadRequestResult()
        {
            //Act
            var result = defaultCitiesController.Get();

            //Assert
            Assert.IsNotInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void Put_When_NeedsAddCityIntoMainList_Then_NotReturnBadRequestResult()
        {
            //Arrange
            var cityName = "Kharkiv";

            //Act
            var result = defaultCitiesController.Put(cityName);

            //Assert
            Assert.IsNotInstanceOf<BadRequestErrorMessageResult>(result);
            mockUof.Verify(a => a.Cities.Update(It.IsAny<City>()), Times.AtLeastOnce);
            mockUof.Verify(a => a.Save(), Times.AtLeastOnce);
        }

        [Test]
        public void Delete_When_NeedsRemoveCityFromMainList_Then_NotReturnBadRequestResult()
        {
            //Arrange
            var cityName = "Kharkiv";

            //Act
            var result = defaultCitiesController.Delete(cityName);

            //Assert
            Assert.IsNotInstanceOf<BadRequestErrorMessageResult>(result);
            mockUof.Verify(a => a.Cities.Update(It.IsAny<City>()), Times.AtLeastOnce);
            mockUof.Verify(a => a.Save(), Times.AtLeastOnce);
        }



        private void GetStubs()
        {
            mockUof.Setup(a => a.Cities.GetAll()).Returns(new List<City>
            {
                new City
                {
                    Name = "Kharkiv"
                }
            });

            mockUof.Setup(a => a.Cities.Find(It.IsAny<Func<City, bool>>())).Returns(new List<City>
            {
                new City()
            });
        }
    }




}