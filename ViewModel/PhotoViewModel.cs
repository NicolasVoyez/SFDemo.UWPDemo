using System.Collections.Generic;

using GalaSoft.MvvmLight.Views;

using SFDemo.UWPDemo.Model;
using SFDemo.UWPDemo.Service;
using Windows.UI.Xaml.Media.Imaging;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.Media.Capture;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SFDemo.UWPDemo.ViewModel
{
    public sealed class PhotoViewModel : ViewModel
    {
        private readonly ISettings _settings;

        public PhotoViewModel(ISettings settings, INavigationService2 navigationService)
            : base(navigationService)
        {
            _settings = settings;
        }

        private SoftwareBitmapSource _takenPhotoSource;
        public SoftwareBitmapSource TakenPhotoSource
        {
            get { return _takenPhotoSource; }
            set
            {
                if (_takenPhotoSource != value)
                {
                    _takenPhotoSource = value;
                    RaisePropertyChangedOnUI(nameof(TakenPhotoSource));
                }
            }
        }

        public ICommand TakePhotoCommand => new RelayCommand(async () => SetBitmapSource(await GetSoftwareBitmap()));

        private async void SetBitmapSource(SoftwareBitmap softwareBitmap)
        {
            if (softwareBitmap == null)
            {
                return ;
            }

            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            TakenPhotoSource = bitmapSource;
        }

        private async Task<SoftwareBitmap> GetSoftwareBitmap()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(300, 300);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return null;
            }

            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            return await decoder.GetSoftwareBitmapAsync();
        }
    }
}
