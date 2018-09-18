using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FinWin.Views;
using FinWin.Views.Base;
using FinWin.Views.Controls;
using MvvmHelpers;
using Xamarin.Forms;

namespace FinWin.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        private string _title;
        public new string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    var titleBinding = GetType().Name.Replace("ViewModel", "_Title");
                    return this[titleBinding];
                }
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }


        private static ViewModelBase _current;
        public static ViewModelBase Current
        {
            get
            {
                if (_current == null)
                {
                    var navStack = App.Current.MainPage.Navigation.NavigationStack;
                    var ret = navStack[navStack.Count - 1].BindingContext as ViewModelBase;
                    return ret;
                }
                return _current;
            }
            set { _current = value; }
        }

        public string this[string index]
        {
            get
            {
                //if (index.Equals("currency"))
                //return Data.currency;
                //var ret = AppResources.ResourceManager.GetString(index, AppResources.Culture);

                return index;
            }
        }

        public virtual void OnPropertyChanged()
        {
            OnPropertyChanged("");
        }

        public virtual void Init()
        {

        }

        public Action<bool> BackEnabledAction { get; set; }

        public void SetBackEnabled(bool enabled)
        {
            BackEnabledAction?.Invoke(enabled);
        }


        #region Navigation Service

        public INavigation FormsNavigation
        {
            get
            {
                var tabController = App.Current.MainPage as TabbedPage;

                if (tabController != null)
                {
                    return tabController.CurrentPage.Navigation;
                }
                else
                {
                    var nav = App.Current.MainPage.Navigation;
                    return nav;
                }
            }
        }

        protected string GetPageName()
        {
            var str = GetType().Name.Replace("ViewModel", "Page");
            return str;
        }

        // View model to view lookup - making the assumption that view model to view will always be 1:1
        private static readonly Dictionary<Type, Type> ViewModelViewDictionary = new Dictionary<Type, Type>();

        #region Register ViewModel & Page
        private static Type GetVMType(Type viewType)
        {
            var vmName = viewType.Namespace.Replace(".Views", ".ViewModels") + "." + viewType.Name.Replace("Page", "ViewModel");
            var ret = Type.GetType(vmName);
            if (ret == null)
                Debug.WriteLine("NavigationService:: ViewModel Not Found: " + vmName);
            return ret;
        }

        public static void RegisterViewModels()
        {
            // Loop through everything in the assembley that implements IViewFor<T>
            var asm = App.Assembly;
            foreach (var viewTypeInfo in asm.DefinedTypes.Where(myType => myType.Name.EndsWith("Page", StringComparison.OrdinalIgnoreCase)))//!dt.IsAbstract && dt.ImplementedInterfaces.Any(ii => ii == typeof(IViewFor))))
            {
                var viewType = viewTypeInfo.AsType();
                var vmType = GetVMType(viewType);
                if (vmType != null)
                    Register(vmType, viewType);
            }
        }

        public static void Register(Type viewModelType, Type viewType)
        {
            if (!ViewModelViewDictionary.ContainsKey(viewModelType))
                ViewModelViewDictionary.Add(viewModelType, viewType);
        }
        #endregion

        #region Navigation
        public async Task PopAsync()
        {
            await FormsNavigation.PopAsync(true);
        }

        public async Task PopModalAsync()
        {
            await FormsNavigation.PopModalAsync(true);
        }
        public void PushInit(Page view)
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            var baseView = view as BasePage;
            if (baseView != null)
                baseView.Init();
           
        }

        public async Task PushAsync<T>(object param = null) where T : ViewModelBase, new()
        {
            var tcs = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var view = InstantiateView<T>(param);
                if (view != null)
                {
                    await FormsNavigation.PushAsync(view, true);
                    PushInit(view);
                    tcs.SetResult(true);
                }
                else
                    tcs.SetResult(false);
            });
            await tcs.Task;
        }

        public async Task PushAsync(Type model, object param = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var view = InstantiateView(model, param);
                if (view != null)
                {
                    await FormsNavigation.PushAsync(view, true);
                    PushInit(view);
                    tcs.SetResult(true);
                }
                else
                    tcs.SetResult(false);
            });
            await tcs.Task;
        }

        public async Task PushModalAsync<T>(object param = null) where T : ViewModelBase, new()
        {
            var tcs = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var view = InstantiateView<T>(param);
                await FormsNavigation.PushModalAsync(view, true);
                PushInit(view);
                tcs.SetResult(true);
            });
            await tcs.Task;
        }

        public async Task PopToRootAsync()
        {
            var navStack = FormsNavigation.NavigationStack;
            if (navStack.Count > 0)
                await FormsNavigation.PopToRootAsync(true);
            else
                App.Current.MainPage = new ThemeNavigationPage(new MainPage());
        }

        private Page InstantiateView<T>(object param = null) where T : ViewModelBase
        {
            try
            {
                // Figure out what type the view model is
                var viewModelType = typeof(T);
                // look up what type of view it corresponds to
                var viewType = ViewModelViewDictionary[viewModelType];

                if (param != null)
                    PageParamAdd(typeof(T).Name, param);
                // instantiate it
                var view = Activator.CreateInstance(viewType);
                return view as Page;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ViewModelBase InstantiateView Error: " + e.Message);
                return null;
            }
        }

        private Page InstantiateView(Type viewModelType, object param = null)
        {
            try
            {
                // look up what type of view it corresponds to
                var viewType = ViewModelViewDictionary[viewModelType];

                if (param != null)
                    PageParamAdd(viewType.Name, param);
                // instantiate it
                var view = Activator.CreateInstance(viewType);
                return view as Page;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ViewModelBase InstantiateView Error: " + e.Message);
                return null;
            }
        }
        #endregion

        #region ViewModel Param
        // View model to param Dictionary
        private static readonly Dictionary<string, object> ViewModelParameter = new Dictionary<string, object>();

        private void PageParamAdd(string page, object param)
        {
            if (!ViewModelParameter.ContainsKey(page))
                ViewModelParameter.Add(page, param);
            else
                ViewModelParameter[page] = param;
        }

        private object PageParamGet(string page)
        {
            object param = null;
            var ret = ViewModelParameter.TryGetValue(page, out param);
            if (ret)
                ViewModelParameter.Remove(page);
            return param;
        }

        public T GetParam<T>()
        {
            return (T)(PageParamGet((GetType()).Name));
        }

        public T GetParam<T>(string viewModelKey)
        {
            return (T)(PageParamGet(viewModelKey));
        }
        #endregion
        #endregion
    }
}