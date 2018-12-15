namespace Spinx.Core.Encryption
{
    public class SecurityHelper
    {
        public static bool VerifyHash(string plainPassword, string encryptedPassword, string salt)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword + salt, encryptedPassword);
        }

        public static string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        public static string GenerateHash(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + salt);
        }
    }
}