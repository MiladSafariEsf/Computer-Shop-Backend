using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    /// <summary>
    /// هش کردن رمز عبور با SHA-256 به همراه Salt تصادفی
    /// </summary>
    /// <param name="password">رمز عبور خام کاربر</param>
    /// <param name="salt">خروجی: مقدار Salt که تولید شده است</param>
    /// <returns>رمز عبور هش شده</returns>
    public static string HashPasswordWithSalt(string password, out string salt)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // تولید Salt تصادفی
            byte[] saltBytes = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToHexString(saltBytes); // تبدیل Salt به رشته‌ی هگزادسیمال

            // ترکیب Salt با پسورد و هش کردن
            string saltedPassword = salt + password;
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

            return Convert.ToHexString(hash); // تبدیل هش به رشته‌ی هگزادسیمال
        }
    }

    /// <summary>
    /// بررسی صحت رمز عبور ورودی با رمز عبور ذخیره شده
    /// </summary>
    /// <param name="enteredPassword">رمز عبوری که کاربر وارد کرده</param>
    /// <param name="storedSalt">Salt ذخیره‌شده در دیتابیس</param>
    /// <param name="storedHashedPassword">رمز عبور هش‌شده‌ی ذخیره‌شده در دیتابیس</param>
    /// <returns>نتیجه بررسی رمز عبور (درست یا نادرست)</returns>
    public static bool VerifyPassword(string enteredPassword, string storedSalt, string storedHashedPassword)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // ترکیب Salt ذخیره‌شده با رمز عبور ورودی و هش کردن
            string saltedPassword = storedSalt + enteredPassword;
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

            // تبدیل هش به رشته‌ی هگزادسیمال و مقایسه با مقدار ذخیره‌شده
            return Convert.ToHexString(hash) == storedHashedPassword;
        }
    }
}
