using DeveloperStore.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace DeveloperStore.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IJsonWebKeySetService _jsonWebKeySetService;

        public AuthController(
                                SignInManager<IdentityUser> signInManager,
                                UserManager<IdentityUser> userManager,
                                IOptions<AppSettings> appSettings,
                                IHttpContextAccessor contextAccessor,
                                IJsonWebKeySetService jsonWebKeySetService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _contextAccessor = contextAccessor;
            _jsonWebKeySetService = jsonWebKeySetService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Regiter(CreateUserViewModel userViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = userViewModel.Email,
                Email = userViewModel.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, userViewModel.Password);

            if (result.Succeeded)
            {
                return CustomResponse(await GenerateToken(userViewModel.Email));
            }

            foreach (var error in result.Errors)
            {
                AddErrorsProcessing(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUserViewModel.Email, loginUserViewModel.Password, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await GenerateToken(loginUserViewModel.Email));
            }

            if (result.IsLockedOut)
            {
                AddErrorsProcessing("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AddErrorsProcessing("Usuário ou senha incorretos");

            return CustomResponse();
        }

        private async Task<UserLoginResponse> GenerateToken(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var identityClaims = await GetUserClaims(user, claims);
            var encodedToken = EncodeToken(identityClaims);

            return GetResponse(user, claims, encodedToken);
        }

        private UserLoginResponse GetResponse(IdentityUser? user, IList<Claim> claims, string encodedToken)
        {
            return new UserLoginResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaim { Type = c.Type, value = c.Value })
                }
            };
        }

        private string EncodeToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _jsonWebKeySetService.GetCurrent(); ;

            var currentIssuer = $"{_contextAccessor.HttpContext?.Request}://{_contextAccessor.HttpContext?.Request.Host}";
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                //Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = key
            });

            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }

        private async Task<ClaimsIdentity> GetUserClaims(IdentityUser? user, IList<Claim> claims)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
#if DEBUG
            userRoles.Add("SysAdm");
            userRoles.Add("Open");
#endif


            //claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);
            return identityClaims;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
