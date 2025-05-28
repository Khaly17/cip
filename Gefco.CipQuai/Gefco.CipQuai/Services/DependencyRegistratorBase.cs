using System;
using System.Collections.Generic;
using System.Text;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace Gefco.CipQuai.Services
{
    /// <summary>
    ///     The children must call the RegisterServices() in their constructor.
    ///     Override the RegisterDependencyService method to add other platform specific implementation.
    /// </summary>
    public abstract class DependencyRegistratorBase
    {
        /// <summary>
        ///     Call in children class constructor. Registers the services.
        ///     Don't forget the base.RegisterDependencyService(); when you override this method.
        /// </summary>
        public virtual void RegisterServices()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            }

            Cleanup();
            RegisterDependencyService();
            RegisterViews();
            RegisterViewModels();
            RegisterDataServices();
        }

        private void Cleanup()
        {

        }

        /// <summary>
        ///     Unregisters the services.
        /// </summary>
        public virtual void UnregisterServices()
        {
        }

        /// <summary>
        ///     Registers the data services.
        /// </summary>
        private void RegisterDataServices()
        {
        }

        #region Registration

        /// <summary>
        ///     Registers the dependency service.
        ///     Don't forget the base.RegisterDependencyService(); when you override this method.
        /// </summary>
        protected virtual void RegisterDependencyService()
        {
            SimpleIoc.Default.Register<IDialogService, ServiceDialog>();
            SimpleIoc.Default.Register<IMyNavigation, ServiceNavigation>();
            //SimpleIoc.Default.Register<IServiceStorage, ServiceStorage>();
        }

        /// <summary>
        ///     Registers the views.
        /// </summary>
        public void RegisterViews()
        {
            SimpleIoc.Default.Register<StartPage.StartPage>();
        }
        public void Reload()
        {
            RegisterViews();
        }

        /// <summary>
        ///     Registers the view models.
        /// </summary>
        private void RegisterViewModels()
        {
            SimpleIoc.Default.Register<StartPage.StartPageViewModel>();
        }

        #endregion
    }
}
