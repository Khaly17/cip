using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Gefco.CipQuai.Services
{
    /// <summary>
    ///     Service navigation.
    /// </summary>
    /// <summary>
    ///     Service navigation.
    /// </summary>
    public class ServiceNavigation : IMyNavigation
    {
        [Preserve]
        public ServiceNavigation()
        {

        }

        public static event EventHandler Navigated;
        //#if DEBUG
        //        private IMyNavigation Navigation => (Application.Current.MainPage as NavigationPage).Navigation;
        //#else            
        private INavigation Navigation => (Application.Current.MainPage as MainPage).Detail.Navigation;
//#endif
        #region INavigation implementation

        public void RemovePage(Page page)
        {
            Navigation?.RemovePage(page);
        }

        public void InsertPageBefore(Page page, Page before)
        {
            Navigation?.InsertPageBefore(page, before);
        }

        public async Task PushAsync(Page page)
        {
            try
            {
                var task = Navigation?.PushAsync(page);
                if (task != null)
                    await task;
                OnNavigated();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public async Task<Page> PopAsync()
        {
            OnNavigated();
            var task = Navigation?.PopAsync();
            return task != null ? await task : await Task.FromResult(null as Page);
        }

        public async Task PopToRootAsync()
        {
            OnNavigated();
            var task = Navigation?.PopToRootAsync();
            if (task != null)
                await task;
        }

        public async Task PushModalAsync(Page page)
        {
            OnNavigated();
            var task = Navigation?.PushModalAsync(page);
            if (task != null)
                await task;
        }

        public async Task<Page> PopModalAsync()
        {
            OnNavigated();
            var task = Navigation?.PopModalAsync();
            return task != null ? await task : await Task.FromResult(null as Page);
        }

        public async Task PushAsync(Page page, bool animated)
        {
            OnNavigated();
            var task = Navigation?.PushAsync(page, animated);
            if (task != null)
                await task;
        }

        public async Task<Page> PopAsync(bool animated)
        {
            OnNavigated();
            var task = Navigation?.PopAsync(animated);
            return task != null ? await task : await Task.FromResult(null as Page);
        }

        public async Task PopToRootAsync(bool animated)
        {
            OnNavigated();
            var task = Navigation?.PopToRootAsync(animated);
            if (task != null)
                await task;
        }

        public void ClearAllButFirstAndLast()
        {
            OnNavigated();
            var pages = Navigation.NavigationStack.ToList();
            foreach (var page in pages)
            {
                if (page != pages.First() && page != pages.Last())
                    Navigation.RemovePage(page);
            }
        }

        public async Task PushModalAsync(Page page, bool animated)
        {
            OnNavigated();
            var task = Navigation?.PushModalAsync(page, animated);
            if (task != null)
                await task;
        }

        public async Task<Page> PopModalAsync(bool animated)
        {
            OnNavigated();
            var task = Navigation?.PopModalAsync(animated);
            return task != null ? await task : await Task.FromResult(null as Page);
        }

        public IReadOnlyList<Page> NavigationStack => Navigation?.NavigationStack;

        public IReadOnlyList<Page> ModalStack => Navigation?.ModalStack;
        public static IMyNavigation Instance => ServiceLocator.Current.GetInstance<IMyNavigation>();

        #endregion

        protected virtual void OnNavigated()
        {
            Navigated?.Invoke(this, EventArgs.Empty);
        }
    }

    public interface IMyNavigation : INavigation
    {
        void ClearAllButFirstAndLast();
    }
}
