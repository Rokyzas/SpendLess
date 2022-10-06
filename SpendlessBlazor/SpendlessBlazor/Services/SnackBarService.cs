using MudBlazor;

namespace SpendlessBlazor.Services
{
    public class SnackBarService
    {
        private ISnackbar snackbar;
        
        public SnackBarService(ISnackbar snackbar)
        {
            this.snackbar = snackbar;
            snackbar.Configuration.PreventDuplicates = false;
            snackbar.Configuration.MaxDisplayedSnackbars = 10;
            snackbar.Configuration.ShowCloseIcon = true;
            snackbar.Configuration.SnackbarVariant = Variant.Outlined;
        }

        public void SuccessMsg(String msg)
        {
            snackbar.Add(msg, Severity.Success, config => { config.VisibleStateDuration = 1000; config.ShowTransitionDuration = 100; config.HideTransitionDuration = 500; });
        }

        public void WarningMsg(String msg)
        {
            snackbar.Add(msg, Severity.Warning, config => { config.VisibleStateDuration = 2000; config.ShowTransitionDuration = 100; config.HideTransitionDuration = 500; });
        }

        public void ErrorMsg(String msg)
        {
            snackbar.Add(msg, Severity.Error, config => { config.VisibleStateDuration = 2000; config.ShowTransitionDuration = 100; config.HideTransitionDuration = 500; });
        }
    }
}
