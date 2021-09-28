using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Firebase.Iid;
using Firebase.Messaging;
using System.IO;
using Android.Content;
using Plugin.CurrentActivity;

namespace OTB_SEGURA.Droid
{
    [Activity(Label = "OTB_SEGURA", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        [Obsolete]
#pragma warning disable CS0809 // El miembro obsoleto invalida un miembro no obsoleto
        protected override void OnCreate(Bundle savedInstanceState)
#pragma warning restore CS0809 // El miembro obsoleto invalida un miembro no obsoleto
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            string dbPath = FileAccess.GetLocalFilePath("EmergencyNumbers.db3");//Crear la tabla del sqlite
            LoadApplication(new App());

            if (!GetString(Resource.String.google_app_id).Equals("1:973479782758:android:f3029c216105cc690cc0a7"))//Validacion de json de datos de firebase
                throw new System.Exception("Invalid Json file");

            FirebaseMessaging.Instance.SubscribeToTopic("all");//suscribiendo dispositivo a topic para recivir notificaciones

            Task.Run(() => {//metodo de firebase mensaging para el token
                var instanceId = FirebaseInstanceId.Instance;
                instanceId.DeleteInstanceId();
                Android.Util.Log.Debug("TAG", "{0} {1}", instanceId.Token, instanceId.GetToken(GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope));

            });
            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            ZXing.Mobile.MobileBarcodeScanner.Initialize(Application);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)//metodo de peticion de los permisos
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            

            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode,
                permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //lector qr

            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);


        }

        public static readonly int idImagen = 1000;

        public TaskCompletionSource<Stream> ImagenTaskCompletionSource { set; get; }
        public static object Instance { get; internal set; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == idImagen)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    ImagenTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    ImagenTaskCompletionSource.SetResult(null);
                }
            }
        }

    }
}