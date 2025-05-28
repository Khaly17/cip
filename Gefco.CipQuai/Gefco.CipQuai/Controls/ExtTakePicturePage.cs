using System;
using System.Threading.Tasks;
using FFImageLoading;
using Gefco.CipQuai.DoubleDeckPage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Gefco.CipQuai.Controls
{
    public abstract class ExtTakePicturePage : ExtContentPage
    {
        public ExtTakePicturePage()
        {
            
        }

        public string Picture { get; set; }
        public bool IsPictureAvailable { get; set; }

        protected abstract void TakePicture(object sender, EventArgs e);
        protected abstract void ChoosePicture(object sender, EventArgs e);

        protected virtual async Task TakePictureAsync()
        {
            MediaFile file = null;
            IsPictureAvailable = false;

            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.ServiceDialog.ShowMessage(":( No camera available.", "No Camera", "OK", null);
                    return;
                }
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Sample",
                    Name = Guid.NewGuid() + ".jpg",
                    MaxWidthHeight = 1200,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    CompressionQuality = 75
                });

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            if (file != null)
            {
                IsPictureAvailable = true;
                await ImageService.Instance.LoadFile(file.Path).PreloadAsync();
                Picture = file.Path;
            }
        }
        protected virtual async Task ChoosePictureAsync()
        {
            MediaFile file = null;
            IsPictureAvailable = false;

            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.ServiceDialog.ShowMessage(":( No camera available.", "No Camera", "OK", null);
                    IsPictureAvailable = false;
                    return;
                }
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    MaxWidthHeight = 1200,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    CompressionQuality = 75,
                });

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            if (file != null)
            {
                IsPictureAvailable = true;
                await ImageService.Instance.LoadFile(file.Path).PreloadAsync();
                Picture = file.Path;
            }
        }
    }
}