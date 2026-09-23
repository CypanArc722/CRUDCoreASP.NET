using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUDCoreASP.NET.Controllers;
using CRUDCoreASP.NET.Models;
using CRUDCoreASP.NET.Services;

namespace usertesting
{
    public class UnitTest1
    {
        #region Arrange
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly Mock<IUserServices> _mockUserServices;
        private readonly HomeController _controller;
        #endregion

        #region Constructor
        public UnitTest1()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockUserServices = new Mock<IUserServices>();

            //dependencies
            _controller = new HomeController(_mockLogger.Object, _mockUserServices.Object);
        }
        #endregion

        #region Methods
        [Fact]
        public async Task Test_Index_Method_Success()
        {
            //ARRANGE
            var mockUsers = new List<Users>
            {
                new Users { Id = 1, FirstName = "Juan", MiddleName = "Santos", LastName = "Dela Cruz" }
            };
            _mockUserServices.Setup(s => s.GetAllUsers()).ReturnsAsync(mockUsers);

            //ACT
            var result = await _controller.Index();

            //ASSERT
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Status>(viewResult.Model);
            Assert.Equal(mockUsers, model.User);
        }

        [Fact]
        public async Task Test_AddUser_Method_Success()
        {
            var newUser = new Users { Id = 2, FirstName = "Pedro", MiddleName = "Gomez", LastName = "Penduko" };
            var mockList = new List<Users> { newUser };
            _mockUserServices.Setup(s => s.CreateUser(newUser)).ReturnsAsync(mockList);
            var result = await _controller.AddUser(newUser);
            var statusResult = Assert.IsType<Status>(result);
            Assert.Equal(mockList, statusResult.User);
        }

        [Fact]
        public async Task Test_EditUser_Method_Success()
        {
            var updatedUser = new Users { Id = 1, FirstName = "Juan Updated", MiddleName = "Santos", LastName = "Dela Cruz" };
            var mockList = new List<Users> { updatedUser };
            _mockUserServices.Setup(s => s.UpdateUser(updatedUser)).ReturnsAsync(mockList);
            var result = await _controller.EditUser(updatedUser);
            var statusResult = Assert.IsType<Status>(result);
            Assert.Equal(mockList, statusResult.User);
        }

        [Fact]
        public async Task Test_DeleteUser_Method_Success()
        {
            int targetId = 1;
            _mockUserServices.Setup(s => s.DeleteUser(targetId)).ReturnsAsync(true);
            var result = await _controller.DeleteUser(targetId);
            var statusResult = Assert.IsType<Status>(result);
            Assert.Equal("Successfully deleted", statusResult.StatusName);
        }

        [Fact]
        public async Task Test_DeleteUser_Method_Failed()
        {
            int targetId = 999;
            _mockUserServices.Setup(s => s.DeleteUser(targetId)).ReturnsAsync(false);
            var result = await _controller.DeleteUser(targetId);
            var statusResult = Assert.IsType<Status>(result);
            Assert.Equal("Failed to delete", statusResult.StatusName);
        }
        #endregion
    }
}
