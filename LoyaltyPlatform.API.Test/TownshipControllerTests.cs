using LoyaltyPlatform.API.Controllers;
using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace LoyaltyPlatform.API.Test
{
    public class TownshipControllerTests
    {
        private readonly Mock<ITownshipRepository> _mockTownshipRepo;
        private readonly TownshipController _controller;

        public TownshipControllerTests()
        {
            _mockTownshipRepo = new Mock<ITownshipRepository>();
            _controller = new TownshipController(_mockTownshipRepo.Object);
        }

        [Fact]
        public void Get_ReturnsOkResult_WithListOfTownships()
        {
            // Arrange
            var townshipList = new List<TownshipDTO>
            {
                new TownshipDTO { Id = 1, Name = "Township 1" },
                new TownshipDTO { Id = 2, Name = "Township 2" }
            };

            var pagingDTO = new TownshipPagingDTO
            {
                Townships = townshipList,
                Paging = new PagingResult { TotalCount = 2, TotalPages = 1 }
            };

            _mockTownshipRepo.Setup(repo => repo.GetAllTownship(It.IsAny<PageSortParam>())).Returns(pagingDTO);
            var pageSortParam = new PageSortParam { PageSize = 10, CurrentPage = 1 };

            // Act
            var result = _controller.Get(pageSortParam);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TownshipPagingDTO>(okResult.Value);
            Assert.Equal(2, returnValue.Townships.Count());
        }

        [Fact]
        public void Get_ReturnsNoContent_WhenNoTownshipsExist()
        {
            // Arrange
            var pagingDTO = new TownshipPagingDTO
            {
                Townships = new List<TownshipDTO>(),
                Paging = new PagingResult { TotalCount = 0, TotalPages = 1 }
            };

            _mockTownshipRepo.Setup(repo => repo.GetAllTownship(It.IsAny<PageSortParam>())).Returns(pagingDTO);

            // Act
            var result = _controller.Get(new PageSortParam { PageSize = 10, CurrentPage = 1 });

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsOkResult_WithTownship()
        {
            // Arrange
            var township = new TownshipDTO { Id = 1, Name = "Township 1" };
            _mockTownshipRepo.Setup(repo => repo.GetTownship(1)).Returns(township);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TownshipDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenTownshipNotExist()
        {
            // Arrange
            _mockTownshipRepo.Setup(repo => repo.GetTownship(1)).Returns((TownshipDTO)null);

            // Act
            var result = _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsCreatedAtAction_WhenTownshipCreated()
        {
            // Arrange
            var township = new TownshipDTO { Id = 1, Name = "Township 1" };
            _mockTownshipRepo.Setup(repo => repo.AddTownship(It.IsAny<TownshipDTO>())).Returns(township);

            // Act
            var result = _controller.Post(township);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<TownshipDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenTownshipIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void Put_ReturnsNoContent_WhenTownshipUpdated()
        {
            // Arrange
            var township = new TownshipDTO { Id = 1, Name = "Updated Township" };
            _mockTownshipRepo.Setup(repo => repo.UpdateTownship(township)).Returns(true);

            // Act
            var result = _controller.Put(1, township);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Put_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var township = new TownshipDTO { Id = 2, Name = "Township 2" }; // Id 2 but updating id 1

            // Act
            var result = _controller.Put(1, township);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenTownshipDeleted()
        {
            // Arrange
            _mockTownshipRepo.Setup(repo => repo.DeleteTownship(1)).Returns(true);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenTownshipNotDeleted()
        {
            // Arrange
            _mockTownshipRepo.Setup(repo => repo.DeleteTownship(1)).Returns(false);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
