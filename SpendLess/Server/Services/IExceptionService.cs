using System.Security.Claims;

namespace SpendLess.Server.Services
{
    public interface IExceptionService
    {
        Task LogException(Exception? ex, ClaimsPrincipal identity);
    }
}