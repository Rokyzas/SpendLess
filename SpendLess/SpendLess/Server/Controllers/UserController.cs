using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("CheckLogin")]
        public async Task<ActionResult<bool>> CheckLogin([FromBody] User? userInfo)
        {
            if (userInfo != null)
            {
                //User userInfo = JsonConvert.DeserializeObject<User>(serializedUser);
                if (userInfo.emailAddress == null || userInfo.password == null)
                {
                    throw new ArgumentNullException(nameof(userInfo));
                }
                else
                {
                    // checking if user exists in database...
                    return true;
                }
            }
            else return false;
        }

        [HttpPost("CreateAccount")]
        public async Task<ActionResult<bool>> CreateAccount(User? userInfo)
        {
            if (userInfo != null)
            {
                if (userInfo.emailAddress == null || userInfo.password == null || userInfo.password == null)
                {
                    throw new ArgumentNullException(nameof(userInfo));
                }
                else
                {
                    // creating new account in database...
                    return true;
                }
            }
            else return false;
        }

    }
}
