using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpendLess.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SpendLess.Server.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly SpendLessContext _context;

        public AuthServices(SpendLessContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAccount(UserDto request)
        {
            if (VerifyRequest(request) && !await VerifyEmail(request.Email!))
            {
                byte[] passwordHash;
                byte[] passwordSalt;
                CreatePasswordHash(request.Password!, out passwordHash, out passwordSalt);
                User newUser = new User
                {
                    Email = request.Email!,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Name = request.Username,
                    InitialBalance = 0
                };
                await _context.Users.AddAsync(newUser);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> VerifyAccount(UserDto request)
        {
            if (VerifyRequest(request))
            {
                if (await VerifyEmail(request.Email!))
                {
                    var passwordHash = _context.Users
                        .Where(user => user.Email.ToLower().Contains(request!.Email!.ToLower()))
                        .Select(user => user.PasswordHash)
                        .FirstOrDefault();
                    var passwordSalt = _context.Users
                        .Where(user => user.Email.ToLower().Contains(request!.Email!.ToLower()))
                        .Select(user => user.PasswordSalt)
                        .FirstOrDefault();
                    await _context.SaveChangesAsync();
                    if (passwordHash == null || passwordSalt == null)
                    {
                        return false;
                    }

                    using (var hmac = new HMACSHA512(passwordSalt!))
                    {
                        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                        bool loll = computedHash.SequenceEqual(passwordHash!);
                        return computedHash.SequenceEqual(passwordHash!);
                    }
                }
            }

            return false;
        }


        private async Task<bool> VerifyEmail(string email)
        {
            return await _context.Users.AnyAsync(o => o.Email == email);
        }

        public string? CreateToken(UserDto user, IConfiguration _configuration)
        {
            try
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddSeconds(15),
                    signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);


                return jwt;
            }
            catch(ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }          
        }

        public bool VerifyRequest(UserDto request)
        {
            if (request != null && request.Email != null && request.Password != null)
            {
                return true;
            }
            return false;
        }

        /*
        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        public void SetRefreshToken(RefreshToken newRefreshToken, HttpResponse Response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }
        */

    }
}