using HelloCMS.LoginApi.Data;
using HelloCMS.LoginApi.Data.Helpers;
using HelloCMS.LoginApi.Data.Models;
using HelloCMS.LoginApi.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HelloCMS.LoginApi.Managers
{
    public class LoginManager
    {
        //Dependency Fields
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly TokenValidationParameters tokenValidationParameters;

        //Constructor
        public LoginManager(UserManager<AppIdentityUser> userManager,
                RoleManager<IdentityRole> roleManager,
                IConfiguration configuration,
                AppDbContext appDbContext,
                TokenValidationParameters tokenValidationParameters)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.appDbContext = appDbContext;
            this.tokenValidationParameters = tokenValidationParameters;
        }

        //Methods

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="registerVM"></param>
        /// <returns> (bool IsValid, string message)
        /// IsValid: Boolen value to show if user successfully created.
        /// message: Success/Error message 
        /// </returns>
        public async Task<(bool IsValid, string message)> RegisterAsync(RegisterVM registerVM)
        {
            //Match Password and Confirm password
            if (registerVM.Password != registerVM.ConfirmPassword)
                throw new ArgumentException("[Confirm Password] should match with [Password]");
            //Validate if Users already exists
            var User = await userManager.FindByNameAsync(registerVM.UserName);
            if (User != null)
                throw new ArgumentException($"User Name {registerVM.UserName} already exists.");
            //Validate if email is already registered
            User = await userManager.FindByEmailAsync(registerVM.Email);
            if (User != null)
                throw new ArgumentException($"User Email {registerVM.Email} already exists.");
            //Validate if user choosen valid Role
            var role = UserRoles.FindByName(registerVM.Role);
            if (role == null)
                throw new ArgumentException($"Role {registerVM.Role} is invalid.");

            //Map View Model to Model
            AppIdentityUser appIdentityUser = new()
            {
                Salutation = registerVM.Salutation,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                SecondaryEmail = registerVM.SecondaryEmail,
                YearOfBirth = registerVM.YearOfBirth,
                Website = registerVM.Website,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            //Create User
            var result = await userManager.CreateAsync(appIdentityUser, registerVM.Password);

            //If user created successfully, add role
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(appIdentityUser, role);
                return (true, "User created successfully!");
            }
            //If user creation failed, capture the error message and send back to controller
            else
            {
                throw new InvalidOperationException(
 $@"User creation failed.
Reason\s:
{result.Errors.Select(a => a.Description).Aggregate((a, b) => a + "\r\n" + b)}");

            }
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<(bool IsAuthorized, TokenResultVM? tokenResultVM)> LoginAsync(LoginVM loginVM)
        {
            var appIdentityUser = await userManager.FindByNameAsync(loginVM.UserName);

            if (appIdentityUser == null || !await userManager.CheckPasswordAsync(appIdentityUser, loginVM.Password)) 
                return (false, null);

            return (true, await GenerateTokenAsync(appIdentityUser, null));
        }

        /// <summary>
        /// Verify and generate token if it is expired
        /// </summary>
        /// <param name="tokenRequestVM"></param>
        /// <returns></returns>
        public async Task<TokenResultVM> VerifyAndGenerateTokenAsync(TokenRequestVM tokenRequestVM)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = appDbContext.RefreshTokens.FirstOrDefault(x => x.Token == tokenRequestVM.RefreshToken);
            var dbUser = await userManager.FindByIdAsync(storedToken.UserId);

            try
            {
                var tokenCheckResult = jwtTokenHandler.ValidateToken(tokenRequestVM.Token, tokenValidationParameters, out var validatedToken);

                return await GenerateTokenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                if (storedToken.DateExpire >= DateTime.UtcNow)
                {
                    return await GenerateTokenAsync(dbUser, storedToken);
                }
                else
                {
                    return await GenerateTokenAsync(dbUser, null);
                }
            }
        }


        //Helper Methods
        private async Task<TokenResultVM> GenerateTokenAsync(AppIdentityUser appIdentityUser, RefreshToken? refreshToken)
        {

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, appIdentityUser.UserName),
                new Claim(ClaimTypes.NameIdentifier, appIdentityUser.Id),
                new Claim(JwtRegisteredClaimNames.Email, appIdentityUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, appIdentityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Add User Role Claims
            var userRoles = await userManager.GetRolesAsync(appIdentityUser);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }


            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if (refreshToken != null)
            {
                var rTokenResultVM = new TokenResultVM(jwtToken, refreshToken.Token, token.ValidTo);
                return rTokenResultVM;
            }

            var newRefreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = appIdentityUser.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };
            await this.appDbContext.RefreshTokens.AddAsync(newRefreshToken);
            await this.appDbContext.SaveChangesAsync();


            var response = new TokenResultVM(jwtToken, newRefreshToken.Token, token.ValidTo);

            return response;
        }


    }
}
