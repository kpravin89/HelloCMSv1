using AutoMapper;
using HelloCMS.Identity.Data;
using HelloCMS.Identity.Data.Models;
using HelloCMS.Identity.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HelloCMS.Identity.Services
{
    public class LoginServices : ILoginServices
    {
        //Dependency Fields
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger<UsersServices> _logger;
        private readonly IMapper _mapper;

        //Constructor
        public LoginServices(UserManager<AppIdentityUser> userManager,
                RoleManager<AppIdentityRole> roleManager,
                IConfiguration configuration,
                AppDbContext appDbContext,
                TokenValidationParameters tokenValidationParameters,
                ILogger<UsersServices> logger,
                IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _appDbContext = appDbContext;
            _tokenValidationParameters = tokenValidationParameters;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TokenResultVM?> LoginAsync(LoginVM loginVM)
        {
            var appIdentityUser = await _userManager.FindByNameAsync(loginVM.UserName);

            if (appIdentityUser == null || !await _userManager.CheckPasswordAsync(appIdentityUser, loginVM.Password))
                return null;

            return await GenerateTokenAsync(appIdentityUser, null);
        }

        /// <summary>
        /// Verify and generate token if it is expired
        /// </summary>
        /// <param name="tokenRequestVM"></param>
        /// <returns></returns>
        public async Task<TokenResultVM> VerifyAndGenerateTokenAsync(TokenRequestVM tokenRequestVM)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = _appDbContext.RefreshTokens.FirstOrDefault(x => x.Token == tokenRequestVM.RefreshToken);
            var dbUser = await _userManager.FindByNameAsync(storedToken.UserName);

            try
            {
                var tokenCheckResult = jwtTokenHandler.ValidateToken(tokenRequestVM.Token, _tokenValidationParameters, out var validatedToken);

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
                new Claim(ClaimTypes.NameIdentifier, appIdentityUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, appIdentityUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, appIdentityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Add User Role Claims
            var userRoles = await _userManager.GetRolesAsync(appIdentityUser);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }


            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
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
                UserName = appIdentityUser.UserName,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };
            await _appDbContext.RefreshTokens.AddAsync(newRefreshToken);
            await _appDbContext.SaveChangesAsync();


            var response = new TokenResultVM(jwtToken, newRefreshToken.Token, token.ValidTo);

            return response;
        }


    }
}
