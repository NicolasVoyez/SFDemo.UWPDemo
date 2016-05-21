
using SFDemo.UWPDemo.Model;
using SFDemo.UWPDemo.Service;
using System.ComponentModel;
using System.Threading;
using System;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;

namespace SFDemo.UWPDemo.ViewModel
{
    public sealed class PositionViewModel : ViewModel
    {
        private readonly ISettings _settings;
        private static Geolocator _geolocator;

        public PositionViewModel(ISettings settings, INavigationService2 navigationService)
            : base(navigationService)
        {
            _settings = settings;
            SetGeoLocator();
        }

        private string _position;
        public string Position
        {
            get { return _position; }
            private set
            {
                if (_position != value)
                {
                    _position = value;                        
                    RaisePropertyChangedOnUI(nameof(Position));
                }
            }
        }

        private string _namedPosition;
        public string NamedPosition
        {
            get { return _namedPosition; }
            private set
            {
                if (_namedPosition != value)
                {
                    _namedPosition = value;
                    RaisePropertyChangedOnUI(nameof(NamedPosition));
                }
            }
        }

        private bool _amIAtSupinfo;
        public bool AmIAtSupinfo
        {
            get { return _amIAtSupinfo; }
            private set
            {
                if (_amIAtSupinfo != value)
                {
                    _amIAtSupinfo = value;
                    RaisePropertyChangedOnUI(nameof(AmIAtSupinfo));
                }
            }
        }

        private void SetGeoLocator()
        {
            if (_geolocator == null)
            {
                // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                _geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };

                SetPositionChanged();
            }

        }

        private async void SetPositionChanged()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            _geolocator.StatusChanged -= _geolocator_StatusChanged;
            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                _geolocator.PositionChanged += OnPositionChanged;
                // Carry out the operation.
                Geoposition pos = await _geolocator.GetGeopositionAsync();

                UpdateLocationData(pos);
            }
            else
            {
                Task.Delay(1000).Wait();
                _geolocator.StatusChanged += _geolocator_StatusChanged;
            }
        }

        private void _geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            SetPositionChanged();
        }

        private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            UpdateLocationData(args.Position);
        }

        private void UpdateLocationData(Geoposition pos)
        {
                Position = $"Lat: {pos.Coordinate.Latitude} \nLon: {pos.Coordinate.Longitude}";
            AmIAtSupinfo = DistanceWithSupinfo(pos.Coordinate) < 500;
        }

        /// <summary>
        /// Lattitude de Supinfo en degrés
        /// </summary>
        private const double SupinfoLat = 48.84225;
        /// <summary>
        /// Longitude de Supinfo en degrés
        /// </summary>
        private const double SupinfoLong = 2.32195;
        // distance en metres
        private double DistanceWithSupinfo(Geocoordinate pos)
        {
            double R = 6371;
            double dLat = DegreesToRadiants(SupinfoLat - pos.Latitude);
            double dLon = DegreesToRadiants(SupinfoLong - pos.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadiants(pos.Latitude)) * Math.Cos(DegreesToRadiants(SupinfoLat)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d*1000;
        }

        private double DegreesToRadiants(double deg)
        {
            return (deg * Math.PI / 180.0);
        }


    }
}
