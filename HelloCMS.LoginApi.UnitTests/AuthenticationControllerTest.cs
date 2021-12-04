using HelloCMS.LoginApi.Controllers;
using HelloCMS.LoginApi.Data.ViewModels;
using HelloCMS.LoginApi.Managers;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HelloCMS.LoginApi.UnitTests
{
    /// <summary>
    /// Perform Unit Testing the Authentication Controller
    /// </summary>
    public class AuthenticationControllerTest
    {
        //Fields
        private readonly Mock<LoginManager> loginManager;

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
            //loginManager.Setup(a => a.RegisterAsync())
            RegisterVM registerVM = new()
            {

            }

            //Act
            var authenticationController = new AuthenticationController(loginManager.Object);
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