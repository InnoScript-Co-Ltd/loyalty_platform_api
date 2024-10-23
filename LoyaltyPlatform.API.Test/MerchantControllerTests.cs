using LoyaltyPlatform.API.Controllers;
using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LoyaltyPlatform.API.Test
{
    public class MerchantControllerTests
    {
        private readonly Mock<IMerchantRepository> _mockMerchantRepo;
        private readonly MerchantController _controller;

        public MerchantControllerTests()
        {
            _mockMerchantRepo = new Mock<IMerchantRepository>();
            _controller = new MerchantController(_mockMerchantRepo.Object);
        }

        [Fact]
        public void Get_ReturnsOkResult_WithListOfMerchants()
        {
            // Arrange
            var merchantList = new List<MerchantDTO>
            {
                new MerchantDTO { Id = 1, Name = "Merchant A" },
                new MerchantDTO { Id = 2, Name = "Merchant B" }
            };

            var pagingDTO = new MerchantPagingDTO
            {
                Merchants = merchantList,
                Paging = new PagingResult { TotalCount = 2, TotalPages = 1 }
            };

            _mockMerchantRepo.Setup(repo => repo.GetAllMerchant(It.IsAny<PageSortParam>())).Returns(pagingDTO);
            var pageSortParam = new PageSortParam { PageSize = 10, CurrentPage = 1 };

            // Act
            var result = _controller.Get(pageSortParam);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<MerchantPagingDTO>(okResult.Value);
            Assert.Equal(2, returnValue.Merchants.Count());
        }

        [Fact]
        public void Get_ReturnsNoContent_WhenNoMerchantsExist()
        {
            // Arrange
            var pagingDTO = new MerchantPagingDTO
            {
                Merchants = new List<MerchantDTO>(),
                Paging = new PagingResult { TotalCount = 0, TotalPages = 1 }
            };

            _mockMerchantRepo.Setup(repo => repo.GetAllMerchant(It.IsAny<PageSortParam>())).Returns(pagingDTO);
            var pageSortParam = new PageSortParam { PageSize = 10, CurrentPage = 1 };

            // Act
            var result = _controller.Get(pageSortParam);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsOkResult_WithMerchant()
        {
            // Arrange
            var merchant = new MerchantDTO { Id = 1, Name = "Merchant A" };
            _mockMerchantRepo.Setup(repo => repo.GetMerchant(1)).Returns(merchant);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<MerchantDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenMerchantNotExist()
        {
            // Arrange
            _mockMerchantRepo.Setup(repo => repo.GetMerchant(1)).Returns((MerchantDTO)null);

            // Act
            var result = _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsCreatedAtAction_WhenMerchantCreated()
        {
            // Arrange
            var merchant = new MerchantDTO { Id = 1, Name = "Merchant A" };
            _mockMerchantRepo.Setup(repo => repo.AddMerchant(It.IsAny<MerchantDTO>())).Returns(merchant);

            // Act
            var result = _controller.Post(merchant);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<MerchantDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var invalidMerchant = new MerchantDTO { Id = 1 }; // Missing required 'Name' field
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = _controller.Post(invalidMerchant);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenMerchantDeleted()
        {
            // Arrange
            _mockMerchantRepo.Setup(repo => repo.DeleteMerchant(1)).Returns(true);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenMerchantNotDeleted()
        {
            // Arrange
            _mockMerchantRepo.Setup(repo => repo.DeleteMerchant(1)).Returns(false);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
