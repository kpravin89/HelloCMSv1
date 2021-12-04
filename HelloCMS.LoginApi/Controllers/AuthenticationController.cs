using HelloCMS.LoginApi.Data;
using HelloCMS.LoginApi.Data.ViewModels;
using HelloCMS.LoginApi.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HelloCMS.LoginApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController
        : ControllerBase
    {

        //Fields

        private readonly LoginManager loginManager;

        //Constructor

        public AuthenticationController(LoginManager loginManager)
        {
            this.loginManager = loginManager;
        }

        //Register Users

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterVM oRegisterVM)
        {
            if(!ModelState.IsValid)
                return BadRequest("Please provide all required Fields...");

            var (Isvalid, message) = await this.loginManager.RegisterAsync(oRegisterVM);

            if (Isvalid)
                return Ok(message);
            return BadRequest(message);
        }

        //Login

        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please provide all required Fields...");

            var (Isvalid, tokenResultVM) = await this.loginManager.LoginAsync(loginVM);

            if (Isvalid)
                return Ok(tokenResultVM);
            else
                return Unauthorized();
        }

        //Refresh

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
