using System;
using FinWin.ViewModels;
using Xamarin.Forms;

namespace FinWin.Views.Base
{
    public class BaseContentPage<T> : BasePage
        where T : ViewModelBase, new()
    {
        public override void Init()
        {
            ViewModel.Init();
            base.Init();
        }

        private T _viewModel;

        public T ViewModel => _viewModel ?? (_viewModel = new T());

        public bool CanGoBack = true;

        ~BaseContentPage()
        {
            _viewModel = null;
        }
        private Command _popRootCommand;
        public Command PopRootCommand
        {
            get
            {
                _popRootCommand = _popRootCommand ?? new Command(DoPopRoot);
                return _popRootCommand;
            }
        }

        protected virtual async void DoPopRoot()
        {
            await ViewModel.PopToRootAsync();
        }

        public void BackEnabled(bool enable)
        {
            if (ViewModel != null)
                CanGoBack = enable;
            NavigationPage.SetHasBackButton(this, enable);
        }

        public BaseContentPage()
        {
            BindingContext = ViewModel;
            SetBinding(TitleProperty, new Binding("Title"));

            NavigationPage.SetHasNavigationBar(this, true);
            ViewModel.BackEnabledAction = BackEnabled;

            BackgroundColor = Color.White;
            if (Device.RuntimePlatform == Device.iOS)
                NavigationPage.SetBackButtonTitle(this, "");
        }
    }
}
