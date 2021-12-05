using HelloCMS.Identity.Data.ViewModels;
using HelloCMS.Identity.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloCMS.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        //Fields

        private readonly ILoginServices _loginServices;

        //Constructor

        public LoginController(ILoginServices loginServices)
        {
            this._loginServices = loginServices;
        }


        //Login

        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            var tokenResultVM = await _loginServices.LoginAsync(loginVM);

            if (tokenResultVM != null)
                return Ok(tokenResultVM);
            else
                return Unauthorized();
        }

        //Refresh

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestVM tokenRequestVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            var result = await _loginServices.VerifyAndGenerateTokenAsync(tokenRequestVM);
            return Ok(result);
        }

    }
}
