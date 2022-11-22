using Microsoft.AspNetCore.Mvc;
using SpendLess.Server.Services;
using SpendLess.Shared;

namespace SpendLess.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly SpendLessContext _context;
        private readonly IAuthServices _services;
        private readonly IConfiguration _configuration;

        public UserController(SpendLessContext context, IAuthServices services, IConfiguration configuration)
        {
            _context = context;
            _services = services;
            _configuration = configuration;

        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] UserDto? request)
        {
            
            if (_services.VerifyRequest(request!))
            {
                Log.Information(request.Email);
                if (!await _services.VerifyAccount(request!))
                {
                    return new LoginResponse(null, "User with this email and password is not found");
                }
                else
                {
                    string token = _services.CreateToken(request!, _configuration);
                    return new LoginResponse(token, "Success");
                }


            }
            else return new LoginResponse(null, "Input not valid");



        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] UserDto? request)
        {
            if (_services.VerifyRequest(request!))
            {
                Log.Information(request.Email);
                if (await _services.CreateAccount(request!))
                {
                    string token = _services.CreateToken(request!, _configuration);
                    return new LoginResponse(token, "Success");
                }
                return new LoginResponse(null, "User already exists");
            }
            return new LoginResponse(null, "Input not valid");

        }

        /*
        [HttpPost("CheckLogin")]
        public async Task<ActionResult<bool>> CheckLogin([FromBody] UserConnect? userInfo)
        {
            if (userInfo != null)
            {
                if (userInfo.emailAddress == null || userInfo.password == null)
                {
                    throw new ArgumentNullException(nameof(userInfo));
                }
                else
                {


                    if (await _context.Users.AnyAsync(o => o.Email == userInfo.emailAddress) && await _context.Users.AnyAsync(o => o.Password == userInfo.password))
                    {
                        return true;
@ -38,40 +92,67 @@
            }
            else return false;
        }
        */
        /*
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!UserServices.user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (UserServices.user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = _services.CreateToken(UserServices.user, _configuration);
            var newRefreshToken = _services.GenerateRefreshToken();
            _services.SetRefreshToken(newRefreshToken, Response);

            return Ok(token);
        }
        */
        ////////////////////////////////////////////////////////
        /*
        [HttpPost("CreateAccount")]
        public async Task<ActionResult<bool>> CreateAccount(UserConnect? userInfo)
        {
            if (userInfo != null)
            {
                if (userInfo.emailAddress == null || userInfo.password == null)
                {
                    throw new ArgumentNullException(nameof(userInfo));
                }
                else
                {

                    if (!await _context.Users.AnyAsync(o => o.Email == userInfo.emailAddress))
                    {



                        User newUser = new User            
                        {
                            Email = userInfo.emailAddress,
                          //  Password = userInfo.password,
                            Name = userInfo.username
                        };
                        _context.Users.Add(newUser);
                        _context.SaveChanges();
                        return true;
                    }
                    else return false;

                    
                    // creating new account in database...
                    
                }
            }
            else return false;
        }
        */

    }
}