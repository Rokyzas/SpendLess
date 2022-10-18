using MudBlazor;

namespace SpendlessBlazor.Services
{
    public static class SnackBarService
    {
        public static ISnackbar snackbar;
        
        public static void InitSnackBarService(ISnackbar sb)
        {
            snackbar = sb;
            snackbar.Configuration.PreventDuplicates = false;
            snackbar.Configuration.MaxDisplayedSnackbars = 10;
            snackbar.Configuration.ShowCloseIcon = true;
            snackbar.Configuration.SnackbarVariant = Variant.Outlined;
        }

        public static void SuccessMsg(String msg = "Success")
        {
            snackbar.Add(msg, Severity.Success, config => { config.VisibleStateDuration = 1000; config.ShowTransitionDuration = 100; config.HideTransitionDuration = 500; });
        }

        public static void WarningMsg(String msg = "Warning")
        {
            snackbar.Add(msg, Severity.Warning, config => { config.VisibleStateDuration = 2000; config.ShowTransitionDuration = 100; config.HideTransitionDuration = 500; });
        }

        public static void ErrorMsg(String msg = "Error")
        {
            snackbar.Add(msg, Severity.Error, config => { config.VisibleStateDuration = 2000; config.ShowTransitionDuration = 100; config.HideTransitionDuration = 500; });
        }
    }
}
