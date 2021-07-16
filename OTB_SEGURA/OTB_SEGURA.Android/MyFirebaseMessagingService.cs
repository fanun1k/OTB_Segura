using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OTB_SEGURA.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    class MyFirebaseMessagingService : FirebaseMessagingService//Clase de envio de notificaciones push
    {
        [Obsolete]
#pragma warning disable CS0809 // El miembro obsoleto invalida un miembro no obsoleto
        public override void OnMessageReceived(RemoteMessage message)
#pragma warning restore CS0809 // El miembro obsoleto invalida un miembro no obsoleto
        {
            base.OnMessageReceived(message);
            SendNotification(message.GetNotification());
        }

        [Obsolete]
        private void SendNotification(RemoteMessage.Notification message)//metodo de generacion de la notificacion de forma local
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.logoOtb)
                .SetContentTitle(message.Title)
                .SetContentText(message.Body)
                .SetAutoCancel(true)
                .SetSound(defaultSoundUri)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}