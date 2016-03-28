using System;

using Windows.UI.Xaml.Controls;

using GalaSoft.MvvmLight.Views;

namespace SFDemo.UWPDemo.Service
{
    public interface INavigationService2 : INavigationService
    {
        event EventHandler Navigated;

        Frame Root { get; set; }
    }
}
