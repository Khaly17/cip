using GalaSoft.MvvmLight.Ioc;
using Gefco.CipQuai.Droid.Services;
using Gefco.CipQuai.Services;

namespace Gefco.CipQuai.Droid
{
    /// <summary>
    /// Dependency registrator.
    /// </summary>
    public sealed class DependencyRegistrator : DependencyRegistratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyRegistrator"/> class.
        /// </summary>
        public DependencyRegistrator()
        {
            this.RegisterServices();
        }

        /// <summary>
        /// Registers the dependency service.
        /// </summary>
        protected override void RegisterDependencyService()
        {
            base.RegisterDependencyService();

            SimpleIoc.Default.Register<IServiceNetwork, ServiceNetwork>();
            SimpleIoc.Default.Register<ILoginService, LoginService>();
            SimpleIoc.Default.Register<IServiceDeviceInfo, ServiceDeviceInfo>();

            SimpleIoc.Default.Register<LoadingPage.ViewModel>();
        }
    }
}