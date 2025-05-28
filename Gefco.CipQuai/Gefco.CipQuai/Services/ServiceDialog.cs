using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Gefco.CipQuai.Services
{
    /// <summary>
    ///     Service dialog.
    /// </summary>
    public class ServiceDialog : IDialogService
    {
        [Preserve]
        public ServiceDialog()
        {

        }

        public static IDialogService Instance => ServiceLocator.Current.GetInstance<IDialogService>();

        /// <summary>
        ///     Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttonText">Button text.</param>
        /// <param name="afterHideCallback">After hide callback.</param>
        /// <returns>Return the error.</returns>
        public async Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            await ShowMessage(message, title, buttonText, afterHideCallback);
        }

        /// <summary>
        ///     Shows the error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttonText">Button text.</param>
        /// <param name="afterHideCallback">After hide callback.</param>
        /// <returns>Return the error.</returns>
        public async Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            await ShowError(error?.Message, title, buttonText, afterHideCallback);
        }

        /// <summary>
        ///     Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <returns>Return the message.</returns>
        public async Task ShowMessage(string message, string title)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            });
        }

        /// <summary>
        ///     Shows the message.
        /// </summary>
        /// <param name="message">the message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttonText">Button text.</param>
        /// <param name="afterHideCallback">After hide callback.</param>
        /// <returns>Return the message.</returns>
        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(title, message, buttonText);
                afterHideCallback?.Invoke();
            });
        }

        /// <summary>
        ///     Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttonConfirmText">Button confirm text.</param>
        /// <param name="buttonCancelText">Button cancel text.</param>
        /// <param name="afterHideCallback">After hide callback.</param>
        /// <returns>Return the message.</returns>
        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            var reponse = await Application.Current.MainPage.DisplayAlert(title, message, buttonConfirmText, buttonCancelText);
            afterHideCallback.Invoke(reponse);
            return reponse;
        }

        /// <summary>
        ///     Shows the message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <returns>Return the message box.</returns>
        public async Task ShowMessageBox(string message, string title)
        {
            await ShowMessage(title, message);
        }
    }
}