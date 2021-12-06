using HelloCMS.Identity.Services;
using HelloCMS.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using HelloCMS.Identity.Data.Dto.Users;

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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto oRegisterVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
            try
            {
                var createdUser = await _usersServices.RegisterAsync(oRegisterVM);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var users = await _usersServices.GetUserAsync(usernameOrEmail);
                return Ok(users);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
            try
            {
                var users = await _usersServices.GetUserAsync(id);
                return Ok(users);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var users = await _usersServices.GetUsersAsync(roles);
                return Ok(users);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateVM)
        {
            try
            {
                await _usersServices.UpdateAsync(id, updateVM);
                return Ok($"User (user ID: {id} updated successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _usersServices.DeleteAsync(id);
                return Ok($"User (user ID: {id} deleted successfully!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
