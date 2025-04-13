using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TumorDetector.Application.Dtos.AccountDtos;
using TumorDetector.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace TumorDetector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser>userManger)
        {
            _userManager = userManger;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto userFromRequest)
        {
            if (ModelState.IsValid)
            {
                //save in database
                ApplicationUser user = new ApplicationUser();
                user.UserName=userFromRequest.UserName;
                user.Email=userFromRequest.Email;
               IdentityResult result= await _userManager.CreateAsync(user, userFromRequest.Password);
                if (result.Succeeded)
                {
                    return Ok("created");
                }
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto userFromRequest)
        {
            if (ModelState.IsValid) {
                //checking if it found or the model is null

                ApplicationUser userFromDb =await _userManager.FindByNameAsync(userFromRequest.userName);
                if (userFromDb != null) {
                    //if i found this username in the db then i will check the password
                    bool found = await _userManager.CheckPasswordAsync(userFromDb, userFromRequest.Password);

                    if (found)
                    {
                        //generate token
                        List<Claim> userClaims = new List<Claim>();
                        //todo:we are here at generating unique id for jwt in 35 minute in eng.cristien video
                        userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id));
                        userClaims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));
                        var UserRoles = await _userManager.GetRolesAsync(userFromDb);
                        foreach (var userRole in UserRoles) {
                            userClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdfASDF1230!!jfkadjkdlaj@#$!#$5"));    //هديله ال key بال byte 

                        var signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);//بتديها الkey وال algorithm


                        //ده كده بعتبر ال head
                        //design for the token
                        JwtSecurityToken myToken = new JwtSecurityToken(
                        #region payload of jwt
                            audience: "http://localhost:4200/",//ده المفروض اللي بتاع ال angular بيشتغل عليه لكن انا مش هكتبه ستاتيك كده بعد كده
                            issuer: "http://localhost:5286/",
                            expires: DateTime.Now.AddHours(1),
                            claims:userClaims
                        #endregion
                            ,
                         #region signature
                            signingCredentials: signingCredential
                        #endregion

                            );

                        // generate token response
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            expiration = DateTime.Now.AddHours(1)//ممكن نكتبها كده myToken.ValidTo(بالمللي ثانيه)

                        }); 
                    }
                         
                }
                ModelState.AddModelError("username", "username or password invalid");
            }

            return BadRequest(ModelState);
        }
    }
}
