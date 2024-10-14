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
    public class StateControllerTests
    {
        private readonly Mock<IStateRepository> _mockStateRepo;
        private readonly StateController _controller;
        public StateControllerTests()
        {
            _mockStateRepo = new Mock<IStateRepository>();
            _controller = new StateController(_mockStateRepo.Object);
        }

        [Fact]
        public void Get_ReturnsOkResult_WhenStateExists()
        {
            // Arrange
            int stateId = 1;
            var expectedState = new StateDTO { Id = stateId, Name = "Test State" };
            _mockStateRepo.Setup(repo => repo.GetState(stateId)).Returns(expectedState);

            // Act
            var result = _controller.Get(stateId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedState = Assert.IsType<StateDTO>(okResult.Value);
            Assert.Equal(expectedState.Id, returnedState.Id);
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenStateDoesNotExist()
        {
            // Arrange
            int stateId = 1;
            _mockStateRepo.Setup(repo => repo.GetState(stateId)).Returns((StateDTO)null);

            // Act
            var result = _controller.Get(stateId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Get_ReturnsBadRequest_WhenIdIsZero()
        {
            // Arrange
            int stateId = 0;

            // Act
            var result = _controller.Get(stateId);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void Get_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            int stateId = 1;
            _mockStateRepo.Setup(repo => repo.GetState(stateId)).Throws(new Exception("Database error"));

            // Act
            var result = _controller.Get(stateId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public void AdddState_ReturnsCreatedAtAction_WhenStateIsAddedSuccessfully()
        {
            // Arrange
            var stateDTO = new StateDTO { Name = "New State" };
            var createdState = new StateDTO { Id = 1, Name = "New State" ,CountryId=1};
            _mockStateRepo.Setup(repo => repo.AddState(stateDTO)).Returns(createdState);

            // Act
            var result = _controller.AdddState(stateDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedState = Assert.IsType<StateDTO>(createdResult.Value);
            Assert.Equal(createdState.Id, returnedState.Id);
            Assert.Equal(createdState.Name, returnedState.Name);
            Assert.Equal("Get", createdResult.ActionName);
            Assert.Equal(1, createdResult.RouteValues["id"]);
        }

        [Fact]
        public void AdddState_ReturnsBadRequest_WhenStateDTOIsNull()
        {
            // Arrange
            StateDTO stateDTO = null;

            // Act
            var result = _controller.AdddState(stateDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void AdddState_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            var stateDTO = new StateDTO { Name = "New State" };
            _mockStateRepo.Setup(repo => repo.AddState(stateDTO)).Throws(new Exception("Database error"));

            // Act
            var result = _controller.AdddState(stateDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public void Put_ReturnsNoContent_WhenStateIsUpdatedSuccessfully()
        {
            // Arrange
            var stateDTO = new StateDTO { Id = 1, Name = "Updated State",CountryId = 1 };
            _mockStateRepo.Setup(repo => repo.UpdateState(stateDTO)).Returns(true);

            // Act
            var result = _controller.Put(1, stateDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Put_ReturnsBadRequest_WhenStateDTOIsNull()
        {
            // Arrange
            StateDTO stateDTO = null;

            // Act
            var result = _controller.Put(1, stateDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Put_ReturnsBadRequest_WhenIdDoesNotMatchStateDTOId()
        {
            // Arrange
            var stateDTO = new StateDTO { Id = 2, Name = "Updated State",CountryId = 1 };

            // Act
            var result = _controller.Put(1, stateDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Put_ReturnsNotFound_WhenStateDoesNotExist()
        {
            // Arrange
            var stateDTO = new StateDTO { Id = 1, Name = "Updated State" };
            _mockStateRepo.Setup(repo => repo.UpdateState(stateDTO)).Returns(false);

            // Act
            var result = _controller.Put(1, stateDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Put_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            var stateDTO = new StateDTO { Id = 1, Name = "Updated State",CountryId = 1 };
            _mockStateRepo.Setup(repo => repo.UpdateState(stateDTO)).Throws(new Exception("Database error"));

            // Act
            var result = _controller.Put(1, stateDTO);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenStateIsDeletedSuccessfully()
        {
            // Arrange
            int stateId = 1;
            _mockStateRepo.Setup(repo => repo.DeleteState(stateId)).Returns(true);

            // Act
            var result = _controller.Delete(stateId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenStateDoesNotExist()
        {
            // Arrange
            int stateId = 1;
            _mockStateRepo.Setup(repo => repo.DeleteState(stateId)).Returns(false);

            // Act
            var result = _controller.Delete(stateId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            int stateId = 1;
            _mockStateRepo.Setup(repo => repo.DeleteState(stateId)).Throws(new Exception("Database error"));

            // Act
            var result = _controller.Delete(stateId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }
        [Fact]
        public void Get_ReturnsOkResult_WithListOfStates()
        {
            // Arrange
            var states = new List<StateDTO>
                {
                    new StateDTO { Id = 1, Name = "New York", CountryId = 1},
                    new StateDTO { Id = 2, Name = "Los Angeles", CountryId = 1 }
                };


            var pagingDTO = new StatePagingDTO
            {
                States = states,
                Paging = new PagingResult { TotalCount = 2, TotalPages = 1 }
            };

            _mockStateRepo.Setup(repo => repo.GetAllState(It.IsAny<PageSortParam>())).Returns(pagingDTO);
            var pageSortParam = new PageSortParam { PageSize = 10, CurrentPage = 1 };

            // Act
            var result = _controller.Get(pageSortParam);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<StatePagingDTO>(okResult.Value);
            Assert.Equal(2, returnValue.States.Count());
        }

        [Fact]
        public void Get_ReturnsNoContent_WhenNoStatesExist()
        {
            // Arrange
            var pagingDTO = new StatePagingDTO
            {
                States = new List<StateDTO>(),
                Paging = new PagingResult { TotalCount = 0, TotalPages = 1 }
            };

            _mockStateRepo.Setup(repo => repo.GetAllState(It.IsAny<PageSortParam>())).Returns(pagingDTO);
            var pageSortParam = new PageSortParam { PageSize = 10, CurrentPage = 1 };

            // Act
            var result = _controller.Get(pageSortParam);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }


    }
}
