using GalaSoft.MvvmLight.Command;
using SFDemo.UWPDemo.Service;
using System;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Calls;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace SFDemo.UWPDemo.ViewModel
{
    public class MusiCallViewModel : ViewModel
    {

        public MusiCallViewModel(INavigationService2 navigationService)
            : base(navigationService)
        {
        }

        private MediaElement _myMusic = null;
        private bool _playing = false;

        public ICommand PlayCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (_myMusic == null)
                    {
                        _myMusic = new MediaElement();
                        _myMusic.AudioCategory = Windows.UI.Xaml.Media.AudioCategory.Media;
                        _myMusic.IsLooping = true;

                        var folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                        var file = await folder.GetFileAsync("music.mp3");
                        var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                        _myMusic.SetSource(stream, file.ContentType);

                    }

                    PhoneCallManager.CallStateChanged += PhoneCallManager_CallStateChanged;
                    _myMusic.Play();
                    _playing = true;
                });
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (_myMusic != null)
                    {
                        PhoneCallManager.CallStateChanged -= PhoneCallManager_CallStateChanged;
                        _myMusic.Pause();
                        _playing = false;
                    }
                });
            }
        }

        private async void PhoneCallManager_CallStateChanged(object sender, object e)
        {
            if (_myMusic == null) return;

            if (PhoneCallManager.IsCallIncoming && _playing)
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,_myMusic.Pause);
                _playing = false;
            }
            else if (!PhoneCallManager.IsCallIncoming && !PhoneCallManager.IsCallActive && !_playing)
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,_myMusic.Play);
                _playing = true;
            }
        }
    }
}
