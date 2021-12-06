using AutoMapper;
using HelloCMS.Identity.Data.Dto.Users;
using HelloCMS.Identity.Data.Helpers;
using HelloCMS.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelloCMS.Identity.Services
{
    public class UsersServices : IUsersServices
    {
        //Dependency Fields
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ILogger<UsersServices> _logger;
        private readonly IMapper _mapper;

        //Constructor
        public UsersServices(UserManager<AppIdentityUser> userManager,
                ILogger<UsersServices> logger,
                IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        //Methods

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="registerVM"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        ///         [Confirm Password] should match with [Password]
        ///         User Name already exists
        ///         User Email ID already Exists
        ///         Invalid Role
        /// </exception>
        /// <exception cref="InvalidOperationException">Any other issue while creating Users in database</exception>
        public async Task<SelectUserDto?> RegisterAsync(RegisterUserDto registerVM)
        {
            string errorMessage = string.Empty;

            //Match Password and Confirm password
            if (registerVM.Password != registerVM.ConfirmPassword)
                errorMessage = "[Confirm Password] should match with [Password]";
            //Validate if Users already exists
            var User = await _userManager.FindByNameAsync(registerVM.UserName);
            if (User != null)
                errorMessage = $"User Name {registerVM.UserName} already exists.";
            //Validate if email is already registered
            User = await _userManager.FindByEmailAsync(registerVM.Email);
            if (User != null)
                errorMessage = $"User Email {registerVM.Email} already exists.";
            //Validate if user choosen valid Role
            var role = UserRoles.FindByName(registerVM.Role);
            if (role == null)
                errorMessage = $"Role {registerVM.Role} is invalid.";

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            //Map View Model to Model (Considering the Similicity, Manual Mapping done instead of using Automapper)
            var appIdentityUser = _mapper.Map<AppIdentityUser>(registerVM);
            //appIdentityUser.UserId = (_userManager.Users.MaxBy(a => a.UserId).UserId) + 1;

            //Create User
            var result = await _userManager.CreateAsync(appIdentityUser, registerVM.Password);

            //If user created successfully, add role
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(appIdentityUser, role);
                var createdUser = await _userManager.FindByNameAsync(registerVM.UserName);
                return _mapper.Map<SelectUserDto>(createdUser);
            }
            //If user creation failed, capture the error message and send back to controller
            else
            {
                errorMessage = "User creation failed with reason:\r\n" + GetIdentityErrors(result.Errors);
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }

        public async Task<SelectUserDto?> GetUser(int Id)
        {
            var userByUserId = await GetById(Id);
            return _mapper.Map<SelectUserDto>(userByUserId);
        }

        public async Task<SelectUserDto?> GetUser(string usernameOrEmail)
        {
            //Find user by User Name
            var usersByUserName = await _userManager.FindByNameAsync(usernameOrEmail);
            if (usersByUserName != null)
                return _mapper.Map<SelectUserDto>(usersByUserName);

            //Find user by User Primary Email ID.
            var usersByEmail = await _userManager.FindByEmailAsync(usernameOrEmail);
            if (usersByEmail != null)
                return _mapper.Map<SelectUserDto>(usersByEmail);

            return null;
        }

        public async Task<List<SelectUserDto>?> GetUsers(string? Roles)
        {
            var RolesList = Roles?.Split(',').Select(a => a.Trim()).ToList();

            //If Roles Provided
            if (RolesList != null)
            {
                RolesList.ForEach(Role =>
                {
                    if (string.IsNullOrEmpty(UserRoles.FindByName(Role)))
                        throw new ArgumentException("Invalid Role/s detected. Please send request with valid Roles");
                });

                var Result = new List<SelectUserDto>();

                RolesList.ForEach(async Role =>
                {
                    var users = await _userManager.GetUsersInRoleAsync(Role);
                    if (users != null)
                    {
                        var selectUserVMList = users.Select(user => _mapper.Map<SelectUserDto>(user)).ToList();
                        Result.AddRange(selectUserVMList);
                    }
                });
                return Result;
            }
            //All users if no roles provided.
            else
            {
                var users = await _userManager.Users.ToListAsync();

                if (users != null)
                    return users.Select(user => _mapper.Map<SelectUserDto>(user)).ToList();
                else
                    return null;

            }

        }

        public async Task Update(int Id, UpdateUserDto updateVM)
        {
            var user = await GetById(Id);
            user = _mapper.Map<AppIdentityUser>(updateVM);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new InvalidOperationException("User update failed with reason\r\n" + GetIdentityErrors(result.Errors));
        }

        public async Task Delete(int Id)
        {
            var user = await GetById(Id);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new InvalidOperationException("User deletion failed with reason\r\n" + GetIdentityErrors(result.Errors));
        }

        //Private Methods

        private async Task<AppIdentityUser> GetById(int Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
                throw new ArgumentException("Given User Id is not exists");
            return user;

        }

        private string GetIdentityErrors(IEnumerable<IdentityError> identityErrors) => identityErrors.Select(a => $"Error Code: {a.Code} Description: {a.Description}").Aggregate((a, b) => a + "\r\n" + b);

        
    }
}
