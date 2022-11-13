using Microsoft.Extensions.Configuration;
using SpendLess.Server.Services;
using SpendLess.Shared;
using SpendLess.Client.Services;
using System.Text.RegularExpressions;

namespace SpendLess.UnitTests
{
    public class Tests
    {
        private AuthServices _serverAuthServices;
        private AuthenticationService _clientAuthServices;
        [SetUp]
        public void Setup()
        {

            _serverAuthServices = new AuthServices(null);
            _clientAuthServices = new AuthenticationService(null, null, null);
        }

        [Test]
        public void Providing_Invalid_Request_Returns_False()
        {
            Assert.IsFalse(_serverAuthServices.VerifyRequest(null));
        }

        [Test]
        public void Creating_Token_When_Aeguments_Are_Invalid_Returns_Null()
        {
            var user = new UserDto
            {
                Email = "email@gmail.com",
                Password = "password",
                Username = "username"
            };
            Assert.IsNull(_serverAuthServices.CreateToken(user, null));
        }

        [Test]
        public void Creating_Hashed_Password_Returns_Two_Bite_Arrays()
        {
            var password = "password";

            _serverAuthServices.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            Assert.IsInstanceOf<byte[]>(passwordHash);
            Assert.IsInstanceOf<byte[]>(passwordSalt);
        }

        [Test]
        public void Providing_Empty_Password_Throws_Message()
        {
            Assert.That(_clientAuthServices.CheckPassword(""), Is.EqualTo("Password is required!"));

        }

        [Test]
        public void Providing_Too_Short_Password_Throws_Message()
        {
            Assert.That(_clientAuthServices.CheckPassword("short"),
                Is.EqualTo("Password must be at least of length 8"));
        }

        [Test]
        public void Providing_Password_Without_Capital_Letters_Throws_Message()
        {
            Assert.That(_clientAuthServices.CheckPassword("no_capital"),
                Is.EqualTo("Password must contain at least one capital letter"));

            Assert.That(_clientAuthServices.CheckPassword("NO_LOWERCASE"),
                Is.EqualTo("Password must contain at least one lowercase letter"));

            Assert.That(_clientAuthServices.CheckPassword("No_Digits"),
                Is.EqualTo("Password must contain at least one digit"));
        }

        [Test]
        public void Providing_Password_Without_Lowercase_Letters_Throws_Message()
        {
            Assert.That(_clientAuthServices.CheckPassword("NO_LOWERCASE"),
                Is.EqualTo("Password must contain at least one lowercase letter"));

            Assert.That(_clientAuthServices.CheckPassword("No_Digits"),
                Is.EqualTo("Password must contain at least one digit"));
        }

        [Test]
        public void Providing_Password_Without_Digits_Throws_Message()
        {
            Assert.That(_clientAuthServices.CheckPassword("No_Digits"),
                Is.EqualTo("Password must contain at least one digit"));
        }

        [Test]
        public void Providing_Empty_Email_Throws_Message()
        {
            Assert.That(_clientAuthServices.CheckEmail(""),
                Is.EqualTo("Email is required"));

        }

        [Test]
        public void Providing_Invalid_Email_Throws_Message()
        {

            Assert.That(_clientAuthServices.CheckEmail("email")!,
                Is.EqualTo("Email format is incorrect"));

            Assert.That(_clientAuthServices.CheckEmail("email@")!,
                Is.EqualTo("Email format is incorrect"));

            Assert.That(_clientAuthServices.CheckEmail("email@com")!,
                Is.EqualTo("Email format is incorrect"));

            Assert.That(_clientAuthServices.CheckEmail("@gmail.com")!,
                Is.EqualTo("Email format is incorrect"));

        }
    }
}