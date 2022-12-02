using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SpendLess.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ExceptionController : ControllerBase
    {
        [HttpPost]
        public async Task LogException([FromBody] Exception? ex)
        {
            if (ex != null)
            {
                try
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    var userClaims = identity.Claims;
                    string email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
                    var expDate = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Expiration)?.Value;
                    if (expDate != null && email != null)
                    {
                        Log.Error($"CLIENT SIDE ERROR\nUser's email: {email}\nToken exp.: {expDate}\nMessage:{ex.Message}\nStack trace: {ex.StackTrace}");
                    }
                    else
                    {
                        Log.Error($"CLIENT SIDE ERROR\nAnonymous user.\nMessage:{ex.Message}\nStack trace: {ex.StackTrace}");
                    }
                }
                catch( Exception ex1)
                {
                    Log.Error($"\nException message: {ex1.Message}\nException stack trace: {ex1.StackTrace}");
                }
                
            }

        }
    }
}
