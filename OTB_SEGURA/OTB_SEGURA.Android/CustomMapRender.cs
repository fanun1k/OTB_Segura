using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OTB_SEGURA.Controls;
using OTB_SEGURA.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(OTB_SEGURA.Droid.CustomMapRender))]
namespace OTB_SEGURA.Droid
{
    class CustomMapRender: MapRenderer
    {
        List<Position> routeCoordinates;


        public CustomMapRender(Context context) : base(context)

        {

        }


        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)

        {

            base.OnElementPropertyChanged(sender, e);

            if (sender != null)
            {

                var formsMap = (CustomMap)sender;

                routeCoordinates = formsMap.RouteCoordinates;

                Control.GetMapAsync(this);

            }

        }


        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {

            base.OnElementChanged(e);

            if (e.OldElement != null)
            {

            }

            if (e.NewElement != null)
            {

                var formsMap = (CustomMap)e.NewElement;

                routeCoordinates = formsMap.RouteCoordinates;

                Control.GetMapAsync(this);

            }

        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {

            base.OnMapReady(map);

            var polylineOptions = new PolylineOptions();

            polylineOptions.InvokeColor(0x66FF0000);

            foreach (var position in routeCoordinates)
            {

                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));

            }

            NativeMap.AddPolyline(polylineOptions);

        }
    }
}