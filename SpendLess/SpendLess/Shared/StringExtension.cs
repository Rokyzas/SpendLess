using System.Security.Cryptography;
using System.Text;

namespace SpendLess.Shared
{
    public static class StringExtension
    {
        public static string? PasswordHash(this string? str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return String.Empty;
            }

            using HashAlgorithm algorithm = SHA256.Create();

            byte[] textData = Encoding.UTF8.GetBytes(str);
            byte[] hash = algorithm.ComputeHash(textData);

            return Encoding.Default.GetString(hash);
        }
    }
}
