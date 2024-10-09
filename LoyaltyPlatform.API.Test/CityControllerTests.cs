using LoyaltyPlatform.API.Controllers;
using LoyaltyPlatform.DataAccess.Implementation;
using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.EntityFramework.EntityModel;
using LoyaltyPlatform.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPlatform.API.Test
{
    public class CityControllerTests
    {
        private readonly Mock<ICityRepository> _mockCityRepo;
        private readonly CityController _controller;
        public CityControllerTests()
        {
            _mockCityRepo = new Mock<ICityRepository>();
            _controller = new CityController(_mockCityRepo.Object);
        }
        [Fact]
        public void GetCity_ReturnsOkResult_WhenCityExists()
        {
            // Arrange
            var cityId = 1;
            var cityDTO = new CityDTO { Id = cityId, Name = "Sample City" };
            _mockCityRepo.Setup(repo => repo.GetCity(cityId)).Returns(cityDTO);

            // Act
            var result = _controller.Get(cityId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCity = Assert.IsType<CityDTO>(okResult.Value);
            Assert.Equal(cityId, returnedCity.Id);
            Assert.Equal("Sample City", returnedCity.Name);
        }

        [Fact]
        public void GetCity_ReturnsNotFoundResult_WhenCityDoesNotExist()
        {
            // Arrange
            var cityId = 1;
            _mockCityRepo.Setup(repo => repo.GetCity(cityId)).Returns((CityDTO)null);

            // Act
            var result = _controller.Get(cityId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public void GetCity_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = _controller.Get(0);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
        [Fact]
        public void GetCity_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var cityId = 1;
            _mockCityRepo.Setup(repo => repo.GetCity(cityId)).Throws(new Exception("Test Exception"));

            // Act
            var result = _controller.Get(cityId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }
        [Fact]
        public void PostCity_ReturnsCreatedAtActionResult_WhenCityIsAddedSuccessfully()
        {
            // Arrange
            var cityDTO = new CityDTO
            {
                Id = 1,
                Name = "Test City",
                CountryId = 1,
                StateId = 1,
            };

            _mockCityRepo.Setup(repo => repo.AddCity(It.IsAny<CityDTO>())).Returns(cityDTO);

            // Act
            var result = _controller.Post(cityDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCity = Assert.IsType<CityDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnedCity.Id);
            Assert.Equal("Test City", returnedCity.Name);
        }

        [Fact]
        public void PostCity_ReturnsBadRequest_WhenCityDTOIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
        [Fact]
        public void PostCity_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var cityDTO = new CityDTO
            {
                Id = 1,
                Name = "Test City",
                CountryId = 1,
                StateId = 1
            };

            _mockCityRepo.Setup(repo => repo.AddCity(It.IsAny<CityDTO>())).Throws(new Exception("Test Exception"));

            // Act
            var result = _controller.Post(cityDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public void PutCity_ReturnsNoContent_WhenCityIsUpdatedSuccessfully()
        {
            // Arrange
            var cityId = 1;
            var cityDTO = new CityDTO
            {
                Id = cityId,
                Name = "Updated City",
                CountryId = 1,
                StateId = 1
            };

            _mockCityRepo.Setup(repo => repo.UpdateCity(It.IsAny<CityDTO>())).Returns(true);

            // Act
            var result = _controller.Put(cityId, cityDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public void PutCity_ReturnsBadRequest_WhenCityDTOIsNull()
        {
            // Act
            var result = _controller.Put(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public void PutCity_ReturnsBadRequest_WhenIdDoesNotMatchCityDTOId()
        {
            // Arrange
            var cityDTO = new CityDTO { Id = 1, Name = "Updated City" };

            // Act
            var result = _controller.Put(2, cityDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void PutCity_ReturnsNotFound_WhenCityIsNotFound()
        {
            // Arrange
            var cityId = 1;
            var cityDTO = new CityDTO
            {
                Id = cityId,
                Name = "Test City",
                CountryId = 1,
                StateId = 1
            };

            _mockCityRepo.Setup(repo => repo.UpdateCity(It.IsAny<CityDTO>())).Returns(false);

            // Act
            var result = _controller.Put(cityId, cityDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PutCity_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var cityId = 1;
            var cityDTO = new CityDTO
            {
                Id = cityId,
                Name = "Test City",
                CountryId = 1,
                StateId = 1
            };

            _mockCityRepo.Setup(repo => repo.UpdateCity(It.IsAny<CityDTO>())).Throws(new Exception("Test Exception"));

            // Act
            var result = _controller.Put(cityId, cityDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public void DeleteCity_ReturnsNoContent_WhenCityIsDeletedSuccessfully()
        {
            // Arrange
            var cityId = 1;

            _mockCityRepo.Setup(repo => repo.DeleteCity(cityId)).Returns(true);

            // Act
            var result = _controller.Delete(cityId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCity_ReturnsNotFound_WhenCityDoesNotExist()
        {
            // Arrange
            var cityId = 1;

            _mockCityRepo.Setup(repo => repo.DeleteCity(cityId)).Returns(false);

            // Act
            var result = _controller.Delete(cityId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteCity_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var cityId = 1;

            _mockCityRepo.Setup(repo => repo.DeleteCity(It.IsAny<int>())).Throws(new Exception("Test Exception"));

            // Act
            var result = _controller.Delete(cityId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public void Get_ReturnsOkResult_WithListOfCities()
        {
            // Arrange
            var cities = new List<CityDTO>
                {
                    new CityDTO { Id = 1, Name = "New York", CountryId = 1, StateId = 1 },
                    new CityDTO { Id = 2, Name = "Los Angeles", CountryId = 1, StateId = 2 }
                };


            var pagingDTO = new CityPagingDTO
            {
                Cities = cities,
                PagingResult = new PagingResult { TotalCount = 2, TotalPages = 1 }
            };

            _mockCityRepo.Setup(repo => repo.GetAllCity(It.IsAny<PageSortParam>())).Returns(pagingDTO);
            var pageSortParam = new PageSortParam { PageSize = 10, CurrentPage = 1 };

            // Act
            var result = _controller.Get(pageSortParam);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CityPagingDTO>(okResult.Value);
            Assert.Equal(2, returnValue.Cities.Count());
        }
        [Fact]
        public void Get_ReturnsNoContent_WhenNoCitiesExist()
        {
            // Arrange
            var pagingDTO = new CityPagingDTO
            {
                Cities = new List<CityDTO>(),
                PagingResult = new PagingResult { TotalCount = 0, TotalPages = 1 }
            };

            _mockCityRepo.Setup(repo => repo.GetAllCity(It.IsAny<PageSortParam>())).Returns(pagingDTO);
            var pageSortParam = new PageSortParam { PageSize = 10, CurrentPage = 1 };

            // Act
            var result = _controller.Get(pageSortParam);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }
      



    }
}
