using System.Security.Cryptography;
using System.Text;

namespace SpendLess.Shared
{
    public class UserConnect
    {

        public string? username { get; set; } = "User";
        public string? emailAddress { get; set; }
        public string? password { get; set; }
        public double balance { get; set; } = 0;

       /* public User(string? username, string? email, string? password)
        {
            this.username = username;
            this.emailAddress = email;
            this.password = password;
            this.balance = 0; 
        }*/
        /* public string? password
         {

             get
             {
                 return password;
             }

             set
             {
                 if (value == null)
                     value = String.Empty;

                 password = value.PasswordHash();
             }
         }
     }*/



    }
}