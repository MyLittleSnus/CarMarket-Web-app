using System;
using System.Security.Cryptography;
using System.Text;

namespace Razor_Test.AdditionalEntities
{
    public class CryptographyHandler
    {
        public string GetSha256Hash(string input)
        {
            var builder = new StringBuilder();

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}

