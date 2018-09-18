using System;
using System.Collections.Generic;
using FinWin.ViewModels;
using FinWin.Views.Base;
using Xamarin.Forms;

namespace FinWin.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            //var pushAsync = (BindingContext as HomeViewModel);
            //pushAsync?.DoStartItemType(StartItemType.Profile);

        }
    }

    public class MainPageXaml : BaseContentPage<MainViewModel>
    {

    }
}
