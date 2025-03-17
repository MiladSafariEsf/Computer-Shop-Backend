using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace SFM.Security
{
    public class JwtService
    {
        private string secretKey;
        public JwtService(IConfiguration configuration)
        {
            secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey is missing in configuration.");
        }
        public async Task<string> GenerateToken(string Number, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                            {
                                    new Claim(ClaimTypes.Name, Number),  // افزودن کلایم نام کاربری
                                    new Claim(ClaimTypes.Role, role)     // افزودن کلایم نقش
                            }),
                Expires = DateTime.UtcNow.AddDays(7),    // زمان انقضای توکن
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)  // نوع امضا
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
        public async Task<ClaimsPrincipal> ValidateToken(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(tokenString, validationParameters, out SecurityToken validatedToken);
                return principal;  // اگر اعتبارسنجی موفق بود، کلایم‌های کاربر بازگردانده می‌شود
            }
            catch
            {
                return null;  // اگر اعتبارسنجی شکست خورد، null بازگردانده می‌شود
            }
        }
        //public async Task<string> RefreshToken(string tokenString)
        //{
        //    var principal = await ValidateToken(tokenString);
        //    if (principal == null)
        //    {
        //        return "Token is expired or invalid.";  // پیام خطای توکن منقضی شده یا غیرمعتبر
        //    }

        //    var username = principal.FindFirst(ClaimTypes.Name)?.Value;
        //    var role = principal.FindFirst(ClaimTypes.Role)?.Value;

        //    // ایجاد توکن جدید با استفاده از متد GenerateToken
        //    string newToken = await GenerateToken(username, role);
        //    return newToken;
        //}
    }
}


