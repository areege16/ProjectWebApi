using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiProject.DTO;
using WebApiProject.Models;

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
