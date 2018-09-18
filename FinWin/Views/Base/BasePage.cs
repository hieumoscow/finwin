using System;
using Xamarin.Forms;

namespace FinWin.Views.Base
{
    public class BasePage : ContentPage
    {
        public virtual void Init()
        {

        }

        public virtual void WillAppear()
        {
            if (Device.RuntimePlatform == Device.iOS)
                App.SetStatusBarDark(!NavigationPage.GetHasNavigationBar(this) && ControlTemplate == null);
        }
    }
}
