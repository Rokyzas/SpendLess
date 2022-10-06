using SpendlessBlazor.Shared;

namespace SpendlessBlazor.Services
{
    public interface IInfoService
    {
        List<Info> ReadJson(SnackBarService snackbar);

    }
}
