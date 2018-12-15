using System.Security.Cryptography;
using System.Text;

namespace Spinx.Core.Lib
{
    public static class Gravatar
    {
        public static string Get(string email, int size = 30)
        {
            return $"https://www.gravatar.com/avatar/{Hash(email)}?size=30&d=mm";
        }

        private static string Hash(string email)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.  
            var md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.  
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));

            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string.  
            foreach (var t in data)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();  // Return the hexadecimal string. 
        }
    }
}
