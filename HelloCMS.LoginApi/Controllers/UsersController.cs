using HelloCMS.Identity.Services;
using HelloCMS.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using HelloCMS.Identity.Data.ViewModels.Users;

namespace HelloCMS.Identity.Controllers
{

    [ApiController]
    //api/<UsersController>
    [Route("api/[controller]")]
    public class UsersController
        : ControllerBase
    {

        //Fields

        private readonly IUsersServices _usersServices;

        //Constructor

        public UsersController(IUsersServices usersServices)
        {
            this._usersServices = usersServices;
        }

        //Create Users
        //POST: api/Users
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserVM oRegisterVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            var createdUser = await _usersServices.RegisterAsync(oRegisterVM);
            return Ok(createdUser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usernameOrEmail">User Name or Email (optional)</param>
        /// <param name="email">Primary User ID  (optional)</param>
        /// <param name="roles">Comma separated Roles (optional)</param>
        /// <returns></returns>
        //GET: api/Users/pravin
        [HttpGet("{usernameOrEmail}")]
        public async Task<IActionResult> GetUser(string usernameOrEmail)
        {
            var users = await _usersServices.GetUser(usernameOrEmail);
            return Ok(users);

        }

        //Get Users base on Role
        /// <summary>
        /// Get list of Users based on the Roles (Comma Separated Roles). In case the Role was not provided, all users will be listed.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns></returns>

        // GET api/Users/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var users = await _usersServices.GetUser(id);
            return Ok(users);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles">Comma separated Roles (optional)</param>
        /// <returns></returns>
        //GET: api/Users?roles=Developer,Manager,Operation
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string? roles)
        {
            var users = await _usersServices.GetUsers(roles);
            return Ok(users);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserVM updateVM)
        {
            await _usersServices.Update(id, updateVM);
            return Ok();

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _usersServices.Delete(id);
            return Ok();
        }

    }
}
