using LoyaltyPlatform.API.Controllers;
using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.API.Test
{
    public class CountryControllerTests
    {
        private readonly Mock<ICountryRepository> _mockCountryRepo;
        private readonly CountryController _controller;

        public CountryControllerTests()
        {
            _mockCountryRepo = new Mock<ICountryRepository>();

            // Setup logger mock (optional if you need to verify logs)
            // Mock<LoggerHelper> mockLogger = new Mock<LoggerHelper>(); 
            // Use constructor with mocked repository
            _controller = new CountryController(_mockCountryRepo.Object);
        }

        [Fact]
        public void Get_ReturnsOkResult_WithListOfCountries()
        {
            // Arrange
            var countryList = new List<CountryDTO>
            {
                new CountryDTO { Id = 1, Name = "USA" },
                new CountryDTO { Id = 2, Name = "Canada" }
            };
            _mockCountryRepo.Setup(repo => repo.GetAllCountry()).Returns(countryList);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CountryDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public void Get_ReturnsNoContent_WhenNoCountriesExist()
        {
            // Arrange
            _mockCountryRepo.Setup(repo => repo.GetAllCountry()).Returns(new List<CountryDTO>());

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsOkResult_WithCountry()
        {
            // Arrange
            var country = new CountryDTO { Id = 1, Name = "USA" };
            _mockCountryRepo.Setup(repo => repo.GetCountry(1)).Returns(country);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CountryDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenCountryNotExist()
        {
            // Arrange
            _mockCountryRepo.Setup(repo => repo.GetCountry(1)).Returns((CountryDTO)null);

            // Act
            var result = _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsCreatedAtAction_WhenCountryCreated()
        {
            // Arrange
            var country = new CountryDTO { Id = 1, Name = "USA" };
            _mockCountryRepo.Setup(repo => repo.AddCountry(It.IsAny<CountryDTO>())).Returns(country);

            // Act
            var result = _controller.Post(country);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<CountryDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenCountryDeleted()
        {
            // Arrange
            _mockCountryRepo.Setup(repo => repo.DeleteCountry(1)).Returns(true);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenCountryNotDeleted()
        {
            // Arrange
            _mockCountryRepo.Setup(repo => repo.DeleteCountry(1)).Returns(false);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
