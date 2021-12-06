using AutoMapper;
using HelloCMS.Identity.Data.Dto.Users;
using HelloCMS.Identity.Data.Models;
using HelloCMS.Identity.Infrastructure.Automapper;
using HelloCMS.Identity.Services;
using HelloCMS.Identity.UnitTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HelloCMS.Identity.UnitTests.Services
{
    public class UsersServicesTest
    {


        private readonly Mock<UserManager<AppIdentityUser>> _userManager;
        private readonly Mock<ILogger<UsersServices>> _logger;
        private readonly IMapper _mapper;

        private readonly UsersServices _usersServices;

        //Constructor
        public UsersServicesTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();

            _logger = new Mock<ILogger<UsersServices>>();

            _userManager = new Mock<UserManager<AppIdentityUser>>(new Mock<IUserStore<AppIdentityUser>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<IPasswordHasher<AppIdentityUser>>().Object,
                     new IUserValidator<AppIdentityUser>[0],
                     new IPasswordValidator<AppIdentityUser>[0],
                     new Mock<ILookupNormalizer>().Object,
                     new Mock<IdentityErrorDescriber>().Object,
                     new Mock<IServiceProvider>().Object,
                     new Mock<ILogger<UserManager<AppIdentityUser>>>().Object);

            _usersServices = new UsersServices(_userManager.Object, _logger.Object, _mapper);
        }


        //Unit Testing Methods



        //Register User Method Testing
        [Fact]
        [Trait("Category", "Users - Register")]
        public async Task Users_Register_Successs()
        {

            //Arrange
            var registerUserDto = new RegisterUserDto("Mr", "Pravinkumar", "Karthikeyan", "pravin", "Test1", "Test1", "Test@gmail.com", "Test@gmail.com", "1900", "TEst Web", "Developer");
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerUserDto);
            var expectedResult = _mapper.Map<SelectUserDto>(appIdentityUser);
            _userManager
                .Setup(a => a.CreateAsync(It.IsAny<AppIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManager
                .SetupSequence(a => a.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((AppIdentityUser)null)
                .ReturnsAsync(appIdentityUser);

            _userManager
                .Setup(a => a.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((AppIdentityUser)null);
            //Act
            var actualResult = await _usersServices.RegisterAsync(registerUserDto);

            //Asset
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        [Trait("Category", "Users - Register")]
        public async Task Users_Register_DuplicationEmailID()
        {
            //Arrange
            var registerUserDto = new RegisterUserDto("Mr", "Pravinkumar", "Karthikeyan", "pravin", "Test1", "Test1", "Test@gmail.com", "Test@gmail.com", "1900", "TEst Web", "Developer");
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerUserDto);
            var expectedResult = _mapper.Map<SelectUserDto>(appIdentityUser);
            _userManager
                .Setup(a => a.CreateAsync(It.IsAny<AppIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManager
                .SetupSequence(a => a.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((AppIdentityUser)null)
                .ReturnsAsync(appIdentityUser);

            _userManager
                .Setup(a => a.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(appIdentityUser);
            //Act
            async Task act() => await _usersServices.RegisterAsync(registerUserDto);

            //Asset
            var argumentException = await Assert.ThrowsAsync<ArgumentException>(act);

            Assert.Equal("User Email Test@gmail.com already exists.", argumentException.Message);
        }

        [Fact]
        [Trait("Category", "Users - Register")]
        public async Task Users_Register_DuplicationUserName()
        {
            //Arrange
            var registerUserDto = new RegisterUserDto("Mr", "Pravinkumar", "Karthikeyan", "pravin", "Test1", "Test1", "Test@gmail.com", "Test@gmail.com", "1900", "TEst Web", "Developer");
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerUserDto);
            var expectedResult = _mapper.Map<SelectUserDto>(appIdentityUser);
            _userManager
                .Setup(a => a.CreateAsync(It.IsAny<AppIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManager
                .Setup(a => a.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(appIdentityUser);

            _userManager
                .Setup(a => a.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((AppIdentityUser)null);
            //Act
            async Task act() => await _usersServices.RegisterAsync(registerUserDto);

            //Asset
            var argumentException = await Assert.ThrowsAsync<ArgumentException>(act);

            Assert.Equal("User Name pravin already exists.", argumentException.Message);
        }

        [Fact]
        [Trait("Category", "Users - Register")]
        public async Task Users_Register_PasswordNotMatching()
        {
            //Arrange
            var registerUserDto = new RegisterUserDto("Mr", "Pravinkumar", "Karthikeyan", "pravin", "Test1", "Test2", "Test@gmail.com", "Test@gmail.com", "1900", "TEst Web", "Developer");
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerUserDto);
            var expectedResult = _mapper.Map<SelectUserDto>(appIdentityUser);
            _userManager
                .Setup(a => a.CreateAsync(It.IsAny<AppIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManager
                .SetupSequence(a => a.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((AppIdentityUser)null)
                .ReturnsAsync(appIdentityUser);

            _userManager
                .Setup(a => a.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((AppIdentityUser)null);
            //Act
            async Task act() => await _usersServices.RegisterAsync(registerUserDto);

            //Asset
            var argumentException = await Assert.ThrowsAsync<ArgumentException>(act);

            Assert.Equal("[Confirm Password] should match with [Password]", argumentException.Message);
        }



        //Get Users
        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_User_ByID_Success()
        {

            //Arrange
            var registerUserDto = new RegisterUserDto("Mr", "Pravinkumar", "Karthikeyan", "pravin", "Test1", "Test1", "Test@gmail.com", "Test@gmail.com", "1900", "TEst Web", "Developer");
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerUserDto);
            var expectedResult = _mapper.Map<SelectUserDto>(appIdentityUser);
            _userManager
                .Setup(a => a.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(appIdentityUser);
            //Act
            var actualResult = await _usersServices.GetUserAsync(0);

            //Asset
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_User_ByID_IDNotExists()
        {
            //Arrange
            var registerUserDto = new RegisterUserDto("Mr", "Pravinkumar", "Karthikeyan", "pravin", "Test1", "Test1", "Test@gmail.com", "Test@gmail.com", "1900", "TEst Web", "Developer");
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerUserDto);
            var expectedResult = _mapper.Map<SelectUserDto>(appIdentityUser);
            _userManager
                .Setup(a => a.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(appIdentityUser);
            //Act
            async Task act() => await _usersServices.RegisterAsync(registerUserDto);

            //Asset
            var argumentException = await Assert.ThrowsAsync<ArgumentException>(act);

            Assert.Equal("Given User Id is not exists", argumentException.Message);
        }

        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_User_ByName_Success()
        {

            //Arrange
            var registerUserDto = new RegisterUserDto("Mr", "Pravinkumar", "Karthikeyan", "pravin", "Test1", "Test1", "Test@gmail.com", "Test@gmail.com", "1900", "TEst Web", "Developer");
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerUserDto);
            var expectedResult = _mapper.Map<SelectUserDto>(appIdentityUser);
            _userManager
                .Setup(a => a.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(appIdentityUser);
            //Act
            var actualResult = await _usersServices.GetUserAsync(0);

            //Asset
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_User_ByEmail_Success()
        {
            //Arrange
            var registerUserDto = new RegisterUserDto("Mr", "Pravinkumar", "Karthikeyan", "pravin", "Test1", "Test1", "Test@gmail.com", "Test@gmail.com", "1900", "TEst Web", "Developer");
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerUserDto);
            var expectedResult = _mapper.Map<SelectUserDto>(appIdentityUser);
            _userManager
                .Setup(a => a.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((AppIdentityUser)null);
            _userManager
                .Setup(a => a.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(appIdentityUser);
            //Act
            var actualResult = await _usersServices.GetUserAsync(0);

            //Asset
            Assert.Equal(expectedResult, actualResult);
        }
        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_User_ByEmailOrName_EmailOrNameNotExists()
        {
            //Arrange
            var registerUserDto = new RegisterUserDto("Mr", "Pravinkumar", "Karthikeyan", "pravin", "Test1", "Test1", "Test@gmail.com", "Test@gmail.com", "1900", "TEst Web", "Developer");
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerUserDto);
            var expectedResult = _mapper.Map<SelectUserDto>(appIdentityUser);
            _userManager
                .Setup(a => a.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((AppIdentityUser)null);
            _userManager
                .Setup(a => a.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((AppIdentityUser)null);
            //Act
            async Task act() => await _usersServices.RegisterAsync(registerUserDto);

            //Asset
            var argumentException = await Assert.ThrowsAsync<ArgumentException>(act);

            Assert.Equal("Given User Id is not exists", argumentException.Message);
        }



        //Get Users
        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_Users_All_Success()
        {


        }

        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_Users_ByRoles_Success()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        [Trait("Category", "Users - Get")]
        public async Task Get_Users_ByRoles_RolesNotExists()
        {

            //Arrange


            //Act


            //Asset
        }


        //Update Users
        [Fact]
        [Trait("Category", "Users - Update")]
        public async Task Users_Update_Success()
        {
  
        }

        [Fact]
        [Trait("Category", "Users - Update")]
        public async Task Users_Update_UserNotExists()
        {

            //Arrange


            //Act


            //Asset
        }



        //Delete Users
        [Fact]
        [Trait("Category", "Users - Delete")]
        public async Task Delete_Users_ByID_Success()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        [Trait("Category", "Users - Delete")]
        public async Task Delete_Users_ByID_UserNotExists()
        {

            //Arrange


            //Act


            //Asset
        }
    }
}
