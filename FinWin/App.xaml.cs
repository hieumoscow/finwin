using System;
using System.Collections.Generic;
using System.Reflection;
using FinWin.ViewModels;
using FinWin.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FinWin
{
    public partial class App : Application
    {
        public new static App Current => Application.Current as App;
        public static Assembly Assembly = typeof(App).GetTypeInfo().Assembly;


        public App()
        {
            InitializeComponent();
            ViewModelBase.RegisterViewModels();

            MainPage = new MainPage();
        }

        public static T GetResource<T>(string key)
        {
            object value = null;
            // do something whenever the "Hi" message is sent
            if (App.Current.Resources.ContainsKey(key))
                App.Current.Resources.TryGetValue(key, out value);
            if (value != null)
            {
                var color = (T)value;
                return color;
            }
            return default(T);

        }

        public static void SetStatusBarDark(bool dark = true)
        {
            Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, dark ? Color.Black : Color.White);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
