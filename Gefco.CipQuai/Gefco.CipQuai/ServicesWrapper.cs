using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using Gefco.CipQuai.Services;

namespace Gefco.CipQuai
{
    public class ServicesWrapper
    {
        public IDialogService ServiceDialog { get; } = ServiceLocator.Current.GetInstance<IDialogService>();
        public IServiceNetwork ServiceNetwork { get; } = ServiceLocator.Current.GetInstance<IServiceNetwork>();
    }
}
