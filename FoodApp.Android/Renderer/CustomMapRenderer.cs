using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Maps;
using FoodApp.Droid.Renderer;
using static AndroidX.ConstraintLayout.Core.Motion.Utils.KeyFrameArray;
using Xamarin.Forms;
using FoodApp.Interface;
using Android.Graphics;
using System.Net;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace FoodApp.Droid.Renderer
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<CustomPin> customPins;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            if(pin?.Label == "driver")
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.delivery));
            }
            else
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.homepin));
            }
            return marker;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                //throw new Exception("Custom pin not found");
                return;
            }

            if (!string.IsNullOrWhiteSpace(customPin.Url))
            {
                var url = Android.Net.Uri.Parse(customPin.Url);
                var intent = new Intent(Intent.ActionView, url);
                intent.AddFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    //throw new Exception("Custom pin not found");
                    return null;
                }

                if (customPin.Name.Equals("driver"))
                {
                    view = inflater.Inflate(Resource.Layout.DriverInfoWindow, null);
                    var driverName = view.FindViewById<TextView>(Resource.Id.driverName);
                    driverName.Text = customPin.DriverName;
                    var driverPhoto = view.FindViewById<ImageView>(Resource.Id.driverPhoto);
                    var imageBitmap = GetImageBitmapFromUrl(customPin.DriverPhoto);
                    driverPhoto.SetImageBitmap(imageBitmap);

                    var vehiclePhoto = view.FindViewById<ImageView>(Resource.Id.vehiclePhoto);
                    var imageBitmap2 = GetImageBitmapFromUrl(customPin.VehiclePhoto);
                    vehiclePhoto.SetImageBitmap(imageBitmap2);

                    var vehicleDetails = view.FindViewById<TextView>(Resource.Id.vehicleDetails);
                    vehicleDetails.Text = customPin.VehicleName;

                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.deliverypin));
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.InfoWindow, null);


                    var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                    var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

                    if (infoTitle != null)
                    {
                        infoTitle.Text = marker.Title;
                    }
                    if (infoSubtitle != null)
                    {
                        infoSubtitle.Text = marker.Snippet;
                    }
                }

                return view;
            }
            return null;
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            try
            {
                Bitmap imageBitmap = null;

                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }

                return imageBitmap;
            }
            catch(Exception)
            {

            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            try
            {
                var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
                foreach (var pin in customPins.ToList())
                {
                    if (pin.Position == position)
                    {
                        return pin;
                    }
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}