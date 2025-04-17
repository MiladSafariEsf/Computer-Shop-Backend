using APICH.API.Models;
using APICH.CORE.Entity;
using APICH.API.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFM.Security;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using APICH.BL.Services.interfaces;
using APICH.API.Models.AAA;


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
            string token;
            token = await jwt.GenerateToken(model.Number, User.Role);
            Response.Cookies.Append("AuthToken", token, cookieOptions);
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
                    Role = Role.User(),
                    Address = model.Address,
                };
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // برای HTTPS فعال باشد
                    SameSite = SameSiteMode.Strict, // جلوگیری از ارسال کوکی در درخواست‌های third-party
                    Expires = DateTime.UtcNow.AddDays(7)
                };
                await userService.AddUser(NewUser);
                string token = await jwt.GenerateToken(NewUser.Number, Role.User());
                Response.Cookies.Append("AuthToken", token, cookieOptions);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return Ok(new { message = "Logged out successfully" });
        }
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return StatusCode(203);
            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var user = await userService.GetByNumber(Number);
            if (PasswordHasher.VerifyPassword(model.oldPassword,user.Salt,user.HashedPassword))
            {
                user.HashedPassword = PasswordHasher.HashPasswordWithSalt(model.newPassword, out string salt);
                user.Salt = salt;
                await userService.UpdateUser();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("ChangeName")]
        public async Task<IActionResult> ChangeName(ChangeNameModel model)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return StatusCode(203);
            var Number = t.FindFirst(ClaimTypes.Name)?.Value;
            var user = await userService.GetByNumber(Number);

            user.UserName = model.Name;
            await userService.UpdateUser();
            return Ok();
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
                Role = t.FindFirst(ClaimTypes.Role).Value,
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
            var role = t.FindFirst(ClaimTypes.Role)?.Value;
            if (role == Role.Admin() || role == Role.Owner())
                return Ok();
            return StatusCode(203, "دسترسی غیر مجاز");
        }
        [HttpPost("IsOwner")]
        public async Task<IActionResult> IsOwner()
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return StatusCode(203);
            if (t.FindFirst(ClaimTypes.Role)?.Value == Role.Owner())
                return Ok();
            return StatusCode(203, "دسترسی غیر مجاز");
        }
        [HttpPut("TuggleAdmin")]
        public async Task<IActionResult> TuggleAdmin(string Number)
        {
            var token = Request.Cookies["AuthToken"];
            var t = await jwt.ValidateToken(token);
            if (t == null)
                return Unauthorized("Invalid or expired token.");

            var rol = t.FindFirst(ClaimTypes.Role)?.Value;
            if (rol != Role.Owner())
                return Forbid("Access denied. Insufficient permissions.");
            return Ok(await userService.TuggleAdminByNumber(Number));
        }
    }
}
