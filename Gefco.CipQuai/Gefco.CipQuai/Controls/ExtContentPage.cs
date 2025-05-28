using System;
using System.Windows.Input;
using CommonServiceLocator;
using Gefco.CipQuai.Services;
using Xamarin.Forms;

namespace Gefco.CipQuai.Controls
{
    public class ExtContentPage : ContentPage
    {
        public static readonly BindableProperty ShowHomeProperty = BindableProperty.Create("ShowHome", typeof(bool), typeof(ExtContentPage), false);
        public static readonly BindableProperty ShowBackOrCloseProperty = BindableProperty.Create("ShowBackOrClose", typeof(bool), typeof(ExtContentPage), true);
        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create("IsLoading", typeof(bool), typeof(ExtContentPage), false);
        public static readonly BindableProperty PageTitleProperty = BindableProperty.Create("PageTitle", typeof(string), typeof(ExtContentPage), "PageTitle here");
        public static readonly BindableProperty PageIconProperty = BindableProperty.Create("PageIcon", typeof(string), typeof(ExtContentPage), "ic_arrow_backy.svg");
        public static readonly BindableProperty NavBarMainCommandProperty = BindableProperty.Create("NavBarMainCommand", typeof(ICommand), typeof(ExtContentPage));
        public static readonly BindableProperty NavBarHomeCommandProperty = BindableProperty.Create("NavBarHomeCommand", typeof(ICommand), typeof(ExtContentPage));
        public static readonly BindableProperty MainContentProperty = BindableProperty.Create("MainContent", typeof(View), typeof(ExtContentPage), null, propertyChanged: OnContentChanged);

        public IMyNavigation NavigationService => ServiceLocator.Current.GetInstance<IMyNavigation>();

        public bool ShowHome
        {
            get => (bool) GetValue(ShowHomeProperty);
            set => SetValue(ShowHomeProperty, value);
        }
        public bool ShowBackOrClose
        {
            get => (bool) GetValue(ShowBackOrCloseProperty);
            set => SetValue(ShowBackOrCloseProperty, value);
        }
        public bool IsLoading
        {
            get => (bool) GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public string PageTitle
        {
            get => (string) GetValue(PageTitleProperty);
            set => SetValue(PageTitleProperty, value);
        }

        public string PageIcon
        {
            get => (string) GetValue(PageIconProperty);
            set => SetValue(PageIconProperty, value);
        }

        public ICommand NavBarMainCommand
        {
            get => (ICommand) GetValue(NavBarMainCommandProperty);
            set => SetValue(NavBarMainCommandProperty, value);
        }

        public ICommand NavBarHomeCommand
        {
            get => (ICommand) GetValue(NavBarHomeCommandProperty);
            set => SetValue(NavBarHomeCommandProperty, value);
        }

        public View MainContent
        {
            get => (View) GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (MainContent != null)
                MainContent.BindingContext = BindingContext;
        }

        private static void OnContentChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ExtContentPage page && page.BindingContext != null && newvalue != null && newvalue is View view)
                view.BindingContext = page.BindingContext;
        }
        protected async void GoHome() { await NavigationService.PopToRootAsync(); }
        protected async void GoHome(object sender, EventArgs e) { await NavigationService.PopToRootAsync(); }
        protected async void GoBack(object sender, EventArgs e) { await NavigationService.PopAsync(); }
    }
}
