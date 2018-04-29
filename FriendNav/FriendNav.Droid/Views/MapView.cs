using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using FriendNav.Core.ViewModels;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;

using FriendNav.Droid.Services;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;

using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Android.Support.V7.App;

using Android.Support.V4.App;
using Android.Support.Design.Widget;

namespace FriendNav.Droid.Views
{
    [Activity(Label = "Map")]
    public class MapView : BaseView,IOnMapReadyCallback,ILocationListener
    {
		const long TWO_SECONDS = 2 * 1000;
        protected override int LayoutResource => Resource.Layout.MapView;
        private  ILocationUpdateService _locationUpdateService;

        public string lattitude = "500";
        public string longitude = "500";

        static readonly int RC_LAST_LOCATION_PERMISSION_CHECK = 1000;
		private GoogleMap GMap;
        private string test;

        MarkerOptions _options;

        internal Button requestLocationUpdatesButton;

        protected LocationManager _locationManager;
        // events, interfaced created in core library 

        View rootLayout;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetUpMap();
            test = "in oncreate";

            _locationManager = (LocationManager)GetSystemService(LocationService);

            _locationUpdateService = Mvx.Resolve<ILocationUpdateService>();

            _options = new MarkerOptions().SetTitle("CurrentPosition");
            requestLocationUpdatesButton = FindViewById<Button>(Resource.Id.request_location_updates_button);

            rootLayout = FindViewById(Resource.Id.root_layout);

            if (_locationManager.AllProviders.Contains(LocationManager.NetworkProvider)
                 && _locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
            {

                requestLocationUpdatesButton.Click += RequestLocationUpdatesButtonOnClick;
            }
            else {
                Log.Debug("FriendNav", "unhandled exception, insufficient permission.");
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == RC_LAST_LOCATION_PERMISSION_CHECK)
            {
                StartRequestingLocationUpdates();
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void RequestLocationUpdatesButtonOnClick(object sender, EventArgs eventArgs)
        {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
                {
                    StartRequestingLocationUpdates();
                    
                }
                else
                {
                    RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
                }
        }

        void StartRequestingLocationUpdates()
        {
            //requestLocationUpdatesButton.SetText(Resource.String.request_location_in_progress_button_text);
            _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, TWO_SECONDS, 1, this);

            var criteria = new Criteria { PowerRequirement = Power.Medium };

            var bestProvider = _locationManager.GetBestProvider(criteria, true);
            var location = _locationManager.GetLastKnownLocation(bestProvider);

            if (location != null)
            {
                double latitude = location.Latitude;
                double longitude = location.Longitude;
                _locationUpdateService.OnLocationChanged(new LocationChangeEventArgs
                {
                    Latitude = latitude.ToString(),
                    Longitude = longitude.ToString()
                });

                this.lattitude = latitude.ToString();
                this.longitude = longitude.ToString();

                LatLng latlng = new LatLng(Convert.ToDouble(this.lattitude), Convert.ToDouble(this.longitude));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);
                GMap.MoveCamera(camera);

                this._options.Dispose();
                this._options = new MarkerOptions().SetPosition(latlng).SetTitle("CurrentPosition");

                GMap.AddMarker(this._options);
            }
        }
        void RequestLocationPermission(int requestCode)
        {

             if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
             {
                 Snackbar.Make(rootLayout, Resource.String.permission_location_rationale, Snackbar.LengthIndefinite)
                         .SetAction(Resource.String.ok,
                                    delegate
                                    {
                                        ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation }, requestCode);
                                    })
                         .Show();
             }
             else
             {
                 ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation }, requestCode);
             }
             
            /*Permission permissionCheck = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);

            if (permissionCheck == Permission.Granted)
            {
                //Execute location service call if user has explicitly granted ACCESS_FINE_LOCATION..
                this.GMap.MyLocationEnabled = true;

                _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, TWO_SECONDS, 1, this);
            }
            else
            {
                Log.Debug("FriendNav", "unhandled exception, insufficient permission.");
            }*/
        }
        private void SetUpMap()
        {
            if (GMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
            {
                StartRequestingLocationUpdates();

            }
            else
            {
                RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
            }

            /* Permission permissionCheck = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);

             if (permissionCheck == Permission.Granted)
             {
                 //Execute location service call if user has explicitly granted ACCESS_FINE_LOCATION..
                 this.GMap.MyLocationEnabled = true;

                 _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, TWO_SECONDS, 1, this);
             }
             else
             {
                 Log.Debug("FriendNav", "unhandled exception, insufficient permission.");
             }*/

        }

        protected override void OnStart()
        {
            base.OnStart();
            _locationManager = GetSystemService(LocationService) as LocationManager;
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            _locationManager.RemoveUpdates(this);
            base.OnPause();
        }

        public void OnLocationChanged(Location location)
        {
            //throw new NotImplementedException();
            if (null != location)
            {
                double latitude = location.Latitude;
                double longitude = location.Longitude;
                _locationUpdateService.OnLocationChanged(new LocationChangeEventArgs
                {
                    Latitude = latitude.ToString(),
                    Longitude = longitude.ToString()
                });

                this.lattitude = latitude.ToString();
                this.longitude = longitude.ToString();

                LatLng latlng = new LatLng(Convert.ToDouble(this.lattitude), Convert.ToDouble(this.longitude));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 15);
                GMap.MoveCamera(camera);

                this._options.Dispose();
                this._options = new MarkerOptions().SetPosition(latlng).SetTitle("CurrentPosition");

                GMap.AddMarker(this._options);
            }
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
            Log.Debug("FriendNav", "The provider " + provider + " is disabled.");
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
            Log.Debug("FriendNav", "The provider " + provider + " is enabled.");
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
            if(status == Availability.OutOfService)
            {
                _locationManager.RemoveUpdates(this);
            }
        }

		
		
    }
}