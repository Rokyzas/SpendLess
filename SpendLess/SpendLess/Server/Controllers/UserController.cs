using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using Newtonsoft.Json;
using SpendLess.Client.Services;
using SpendLess.Shared;

namespace SpendLess.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly SpendLessContext _context;

        public UserController (SpendLessContext context)
        {
            _context = context;
        }  

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
                    }
                    else return false;                      
                }
            }
            else return false;
        }

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
                            Password = userInfo.password,
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

    }
}
