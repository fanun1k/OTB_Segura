using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OTB_SEGURA.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    [Obsolete]
    class MyFirebaseIdService : FirebaseInstanceIdService //clase de manejo de la base de datos para datos de notificaciones
    {
        [Obsolete]
        public override void OnTokenRefresh()//metodo de la actualizacion del token de la BBD
        {
            base.OnTokenRefresh();
            Android.Util.Log.Debug("Refreshed Token:", FirebaseInstanceId.Instance.Token);
        }
    }
}