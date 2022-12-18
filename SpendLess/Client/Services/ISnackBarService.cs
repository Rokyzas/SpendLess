namespace SpendLess.Client.Services
{
    public interface ISnackBarService
    {
        void ErrorMsg(string msg);
        void SuccessMsg(string msg);
        void WarningMsg(string msg);
    }
}