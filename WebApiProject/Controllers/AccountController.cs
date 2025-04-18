using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiProject.DTO;
using WebApiProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Constructor
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configur;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configur)
        {
            this.userManager = userManager;
            this.configur = configur;
        }
        #endregion


        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("/signin-google")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return BadRequest("Google authentication failed.");

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;

            // Check if the user exists in the database
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };
                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest("Error creating user.");
                }
            }

            // Generate JWT
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName)
    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configur["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configur["JWT:Iss"],
                audience: configur["JWT:Aud"],
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { message = "Google Login Success", token = jwt, expired = DateTime.Now.AddDays(2) });
        }


        #region Register
        //Register
        [HttpPost("register")]//api/account/register
        public async Task<IActionResult> Register(RegisterDTO UserFormConsumer)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = UserFormConsumer.UserName,
                    Email = UserFormConsumer.Email
                };
                IdentityResult result = await userManager.CreateAsync(user, UserFormConsumer.Password);
                if (result.Succeeded)
                {
                    return Ok("Account Create Success");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return BadRequest(ModelState);

        }

        #endregion

        #region Login
        [HttpPost("login")]//api/account/login
        public async Task<IActionResult> Login(LoginDto UserFormConsumer)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(UserFormConsumer.UserName);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, UserFormConsumer.Password);
                    if (found)
                    {
                        var userRoles = await userManager.GetRolesAsync(user);

                        //create token

                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        if (userRoles != null)
                        {
                            foreach (var role in userRoles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }
                        ///--------------------------------------------------
                        SymmetricSecurityKey signinkey = new(Encoding.UTF8.GetBytes(configur["JWT:Key"]));

                        SigningCredentials signingCredentials =
                            new SigningCredentials(signinkey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: configur["JWT:Iss"],
                            audience: configur["JWT:Aud"],
                            expires: DateTime.Now.AddDays(2),
                            claims: claims,
                            signingCredentials: signingCredentials
                            );

                        //compact 
                        return Ok(new
                        {
                            expired = DateTime.Now.AddDays(2),
                            token = new JwtSecurityTokenHandler().WriteToken(token)
                        });
                    }
                }
                ModelState.AddModelError("", "Invalid Account");
            }
            return BadRequest(ModelState);
        } 
        #endregion

    }
}
