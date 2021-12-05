using HelloCMS.Identity.Services;
using HelloCMS.Identity.Controllers;
using Moq;
using System.Threading.Tasks;
using Xunit;
using HelloCMS.Identity.Data.ViewModels.Users;

namespace HelloCMS.Identity.UnitTests
{
    /// <summary>
    /// Perform Unit Testing the Authentication Controller
    /// </summary>
    public class AuthenticationControllerTest
    {
        //Fields
        private readonly Mock<UsersServices> loginManager;

        //Constructor
        public AuthenticationControllerTest( )
        {
            loginManager = new();
        }

        //Unit Testing Methods
        [Fact]
        public async Task Get_Authentication_Register_Success()
        {   
            //Arrange
            loginManager.Setup(a => a.RegisterAsync(It.IsAny<RegisterUserVM>()));

            //Act
            var authenticationController = new LoginController(loginManager.Object);
            var actionResult = authenticationController.Register();


            //Asset
        }

        [Fact]
        public async Task Get_Authentication_Register_Bad_Request()
        {
            //Arrange


            //Act


            //Asset

        }
    }
}