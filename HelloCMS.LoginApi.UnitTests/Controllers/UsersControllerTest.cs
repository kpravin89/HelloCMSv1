using HelloCMS.Identity.Services;
using HelloCMS.Identity.Controllers;
using Moq;
using System.Threading.Tasks;
using Xunit;
using HelloCMS.Identity.Data.Dto.Users;
using Microsoft.AspNetCore.Identity;
using HelloCMS.Identity.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HelloCMS.Identity.UnitTests
{
    /// <summary>
    /// Perform Unit Testing the Authentication Controller
    /// </summary>
    public class UsersControllerTest 
    {
        //Fields
        private readonly Mock<IUsersServices> usersServicesMoq;

        private readonly RegisterUserDto registerUserDto = new ("", "", "", "Test", "", "", "Test", "", "", "", "");
        private readonly SelectUserDto selectUserDto = new(0, "", "", "", "", "", "", "", "");
        private readonly UpdateUserDto updateUserDto = new ("", "", "", "Test", "", "");

        //Constructor
        public UsersControllerTest( )
        {
            usersServicesMoq = new Mock<IUsersServices>();
        }

        //Unit Testing Methods

        //Register Users
        [Fact]
        [Trait("Category", "Users - Register")]
        public async Task Post_Users_Register_Success()
        {
            //Arrange
            
            usersServicesMoq
                .Setup(a => a.RegisterAsync(registerUserDto))
                .ReturnsAsync(selectUserDto);
            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.RegisterUser(registerUserDto);

            //Asset
            var OkResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<SelectUserDto>(OkResult.Value);
            Assert.Equal(selectUserDto, returnValue);
        }

        [Fact]
        [Trait("Category", "Users - Register")]
        public async Task Post_Users_Register_Bad_Request()
        {
            //Arrange
            usersServicesMoq
                .Setup(a => a.RegisterAsync(registerUserDto))
                .Throws<ArgumentException>();

            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.RegisterUser(registerUserDto);

            //Asset
            Assert.IsType<BadRequestObjectResult>(result);

        }


        [Fact]
        [Trait("Category", "Users - Update")]
        public async Task Put_Users_Update_Success()
        {
            //Arrange
            usersServicesMoq
                .Setup(a => a.UpdateAsync(0, updateUserDto))
                .Verifiable();
            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.UpdateUser(0, updateUserDto);

            //Asset
            var OkResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        [Trait("Category", "Users - Update")]
        public async Task Put_Users_Update_Bad_Request()
        {
            //Arrange
            usersServicesMoq
                .Setup(a => a.UpdateAsync(0, updateUserDto))
                .Throws<ArgumentException>();
            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.UpdateUser(0, updateUserDto);

            //Asset
            Assert.IsType<BadRequestObjectResult>(result);

        }

        [Fact]
        [Trait("Category", "Users - Delete")]
        public async Task Delete_Users_Delete_Success()
        {
            //Arrange
            usersServicesMoq
                .Setup(a => a.DeleteAsync(0))
                .Verifiable();
            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.DeleteUser(0);

            //Asset
            var OkResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        [Trait("Category", "Users - Delete")]
        public async Task Delete_Users_Delete_Bad_Request()
        {
            //Arrange
            usersServicesMoq
                .Setup(a => a.DeleteAsync(0))
                .Throws<ArgumentException>();
            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.DeleteUser(0);

            //Asset
            Assert.IsType<BadRequestObjectResult>(result);

        }

        //Get Users
        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_Users_ById_Success()
        {
            //Arrange            
            usersServicesMoq
                .Setup(a => a.GetUserAsync(0))
                .ReturnsAsync(selectUserDto);

            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.GetUser(0);

            //Asset
            var OkResult = Assert.IsType<OkObjectResult>(result);
            var resultObject = OkResult.Value;
            Assert.Equal(selectUserDto, resultObject);
        }

        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_Users_ById_Bad_Request()
        {
            //Arrange
            usersServicesMoq
                .Setup(a => a.GetUserAsync(0))
                .Throws<ArgumentException>();
            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.GetUser(0);

            //Asset
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_Users_ByNameOrEmail_Success()
        {
            //Arrange
            usersServicesMoq
                .Setup(a => a.GetUserAsync(""))
                .ReturnsAsync(selectUserDto);

            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.GetUser("");

            //Asset
            var OkResult = Assert.IsType<OkObjectResult>(result);
            var resultObject = OkResult.Value;
            Assert.Equal(selectUserDto, resultObject);
        }

        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_Users_ByNameOrEmail_Bad_Request()
        {
            //Arrange
            usersServicesMoq
                .Setup(a => a.GetUserAsync(""))
                .Throws<ArgumentException>();
            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.GetUser("");

            //Asset
            Assert.IsType<BadRequestObjectResult>(result);

        }
    }
}