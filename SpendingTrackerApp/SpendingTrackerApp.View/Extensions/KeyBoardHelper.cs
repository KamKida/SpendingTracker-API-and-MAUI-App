#if ANDROID
using Android.Content;
using Android.Views.InputMethods;
#endif

namespace SpendingTrackerApp.Extensions
{
    public static class KeyboardHelper
    {
        public static void HideKeyboard()
        {
#if ANDROID
            var activity = Platform.CurrentActivity;
            if (activity == null)
                return;

            var inputMethodManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
            var token = activity.CurrentFocus?.WindowToken;

            if (token != null)
            {
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
            }
#endif
        }
    }
}
