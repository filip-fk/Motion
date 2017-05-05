using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.UI.Core;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Motion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            request_loc();
        }

        public async void request_loc()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            var _geolocator = new Geolocator();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    // Create Geolocator and define perodic-based tracking (2 second interval).
                    _geolocator = new Geolocator { ReportInterval = 2000 };

                    // Subscribe to the PositionChanged event to get location updates.
                    _geolocator.PositionChanged += OnPositionChanged;

                    break;

                case GeolocationAccessStatus.Denied:
                    //Denied
                    break;

                case GeolocationAccessStatus.Unspecified:
                    //Unspecified
                    break;
            }
        }

        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                //get location

                Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 1 };

                // Carry out the operation.
                Geoposition pos = await geolocator.GetGeopositionAsync();
                UpdateLocationData(pos);
            });
        }

        double lat = 0;
        double log = 0;

        double lat1 = 0;
        double log1 = 0;

        public void UpdateLocationData(Geoposition pos)
        {
            if (lat1 == 0)
            {
                if (lat == 0)
                {
                    lat = pos.Coordinate.Point.Position.Latitude;
                    log = pos.Coordinate.Point.Position.Longitude;
                }

                else
                {
                    lat1 = pos.Coordinate.Point.Position.Latitude;
                    log1 = pos.Coordinate.Point.Position.Longitude;
                }
            }

            if (lat1 != 0)
            {
                lat1 = 0;
                log1 = 0;

                lat = pos.Coordinate.Point.Position.Latitude;
                log = pos.Coordinate.Point.Position.Longitude;
            }

            check_speed(pos);
        }

        public void check_speed(Geoposition pos2)
        {
            int lat_f = 0;
            int log_f = 0;


            if (lat > lat1)
            {
                lat_f = Convert.ToInt32(lat - lat1);
            }

            else lat_f = Convert.ToInt32(lat1 - lat);

            if (log > log1)
            {
                log_f = Convert.ToInt32(log - log1);
            }

            else log_f = Convert.ToInt32(log1 - log);

            double s = Math.Sqrt(lat_f ^ 2 + log_f ^ 2);

            double v = Math.Round((((s / 2) / 60) / 60), 2);

            //speed.Text = v.ToString();
            speed.Text = pos2.Coordinate.Speed.ToString();
            longitude.Text = Math.Round(log, 4).ToString();
            latitude.Text = Math.Round(lat, 4).ToString();
            source.Text = pos2.Coordinate.PositionSource.ToString();
        }
    }
}
