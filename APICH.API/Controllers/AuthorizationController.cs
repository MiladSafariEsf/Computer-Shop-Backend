using APICH.API.Models;
using APICH.CORE.Entity;
using APICH.API.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFM.Security;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using APICH.BL.Services.interfaces;

namespace APICH.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly JwtService jwt;
        private readonly IUserService userService;

        public AuthorizationController(JwtService jwt, IUserService userService)
        {
            this.jwt = jwt;
            this.userService = userService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var User = await userService.GetByNumber(model.Number);
            if (User == null || !PasswordHasher.VerifyPassword(model.Password,User.Salt,User.HashedPassword))
                return BadRequest();
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // برای HTTPS فعال باشد
                SameSite = SameSiteMode.Strict, // جلوگیری از ارسال کوکی در درخواست‌های third-party
                Expires = DateTime.UtcNow.AddDays(7)
            };
            TokenModel token;
            if (User.IsAdmin == true)
            {
                token = new TokenModel()
                {
                    token = await jwt.GenerateToken(model.Number, Role.Admin())
                };
            }
            else
            {
                token = new TokenModel()
                {
                    token = await jwt.GenerateToken(model.Number, Role.User())
                };
            }
            Response.Cookies.Append("AuthToken", token.token, cookieOptions);
            return Ok("Login was success full!");
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var User = await userService.GetByNumber(model.Number);
            if (User == null)
            {
                var NewUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Number = model.Number,
                    UserName = model.UserName,
                    HashedPassword = PasswordHasher.HashPasswordWithSalt(model.Password, out string salt),
                    Salt = salt,
                    CreatedAt = DateTime.Now,
                    IsAdmin = false,
                };
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // برای HTTPS فعال باشد
                    SameSite = SameSiteMode.Strict, // جلوگیری از ارسال کوکی در درخواست‌های third-party
                    Expires = DateTime.UtcNow.AddDays(7)
                };
                await userService.AddUser(NewUser);
                var token = new TokenModel()
                {
                    token = await jwt.GenerateToken(NewUser.Number, Role.User())
                };
                Response.Cookies.Append("AuthToken", token.token, cookieOptions);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("ValidationToken")]
        public async Task<IActionResult> ValidationToken(TokenModel model)
        {
            var t = await jwt.ValidateToken(model.token);
            if (t == null)
                return BadRequest("VallidationError");

            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var User = await userService.GetByNumber(Number);
            var role = t.FindFirst(ClaimTypes.Role)?.Value;
            var m = new TokenUserInfo()
            {
                number = User.Number,
                username = User.UserName
            };
            return Ok(m);
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return Ok(new { message = "Logged out successfully" });
        }
        [HttpPost("GetUserData")]
        public async Task<IActionResult> GetUserData()
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return StatusCode(203);
            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var user = await userService.GetByNumber(Number);
            var model = new UserDataModel()
            {
                number = user.Number,
                username = user.UserName,
            };
            return Ok(model);
        }
        [HttpPost("IsAdmin")]
        public async Task<IActionResult> IsAdmin()
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return StatusCode(203);
            if (t.FindFirst(ClaimTypes.Role)?.Value == Role.Admin())
                return Ok();
            return StatusCode(203, "دسترسی غیر مجاز");
        }
    }
}
