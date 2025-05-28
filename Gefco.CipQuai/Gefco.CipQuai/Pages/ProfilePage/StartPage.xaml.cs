
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.ProfilePage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage
    {
        public StartPage()
        {
            InitializeComponent();
            
        }

        public ViewModel ViewModel => BindingContext as ViewModel;

        protected void ShowPictureMenu(object sender, EventArgs e)
        {
            BusyIndicator.IsVisible = true;
            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 250, 0, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame));
        }

        protected override async void TakePicture(object sender, EventArgs e)
        {
            CancelPictureMenu(null, null);
            ViewModel.IsLoading = true;

            await base.TakePictureAsync();

            if (IsPictureAvailable)
            {
                ViewModel.User.ProfilePicture.PicturePath = Picture;
                ViewModel.UserPicture = Picture;
                ViewModel.SaveUserPicture();
            }

            ViewModel.IsLoading = false;
        }

        protected override async void ChoosePicture(object sender, EventArgs e)
        {
            CancelPictureMenu(null, null);
            ViewModel.IsLoading = true;

            await base.ChoosePictureAsync();

            if (IsPictureAvailable)
            {
                ViewModel.User.ProfilePicture.PicturePath = Picture;
                ViewModel.UserPicture = Picture;
                ViewModel.SaveUserPicture();
            }

            ViewModel.IsLoading = false;
        }

        private void CancelPictureMenu(object sender, EventArgs e)
        {
            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 0, 250, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 1, 0, Easing.SpringIn);
            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame), finished: (d, b) => BusyIndicator.IsVisible = false);
        }
    }
}