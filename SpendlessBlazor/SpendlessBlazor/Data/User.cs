using MudBlazor;
using SpendlessBlazor.Services;

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

                password = value.passwordHash();
            }
        }
    }

    public static class StringExtension
    {
        public static string passwordHash(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return String.Empty;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(str);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }
    }

}
