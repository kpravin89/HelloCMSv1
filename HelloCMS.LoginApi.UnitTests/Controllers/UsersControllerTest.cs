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

        //Constructor
        public UsersControllerTest( )
        {
            usersServicesMoq = new Mock<IUsersServices>();
        }

        //Unit Testing Methods
        [Fact]
        public async Task Post_Users_Register_Success()
        {
            //Arrange
            var registerUserDto = new RegisterUserDto("", "", "", "Test", "", "", "Test", "", "", "", "");
            SelectUserDto selectUserDto = new(0, "", "", "", "", "", "", "", "");
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
        public async Task Post_Users_Register_Bad_Request()
        {
            //Arrange
            var registerUserDto = new RegisterUserDto("", "", "", "Test", "", "", "Test", "", "", "", "");
            SelectUserDto selectUserDto = new(0, "", "", "", "", "", "", "", "");
            usersServicesMoq
                .Setup(a => a.RegisterAsync(registerUserDto))
                .Throws<ArgumentException>();

            var usersController = new UsersController(usersServicesMoq.Object);

            //Act
            var result = await usersController.RegisterUser(registerUserDto);

            //Asset
            Assert.IsType<BadRequestResult>(result);

        }
    }
}