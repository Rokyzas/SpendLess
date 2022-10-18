using MudBlazor;
using SpendlessBlazor.Services;
using System.Security.Cryptography;
using System.Text;

namespace SpendlessBlazor.Data
{
    public static class User
    {

        public static string? username { get; set; } = "User";
        public static string? emailAddress { get; set; }
        private static string? password { get; set; }
        public static double balance { get; set; } = 0;

        
        public static string? Password {

            get
            {
                return password;
            }

            set
            {
                if(value == null)   
                    value = String.Empty;

                password = value.PasswordHash();
            }
        }
    }

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
