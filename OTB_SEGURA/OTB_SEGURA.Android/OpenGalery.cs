using Android.Content;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(OTB_SEGURA.Droid.OpenGalery))]
namespace OTB_SEGURA.Droid
{
    public class OpenGalery : IOpenGalery
    {
        public Task<Stream> GetFotoAsync()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            MainActivity activity = Forms.Context as MainActivity;

            activity.StartActivityForResult(Intent.CreateChooser(intent, "Selecciona una Imagen"), MainActivity.idImagen);

            activity.ImagenTaskCompletionSource = new TaskCompletionSource<Stream>();
            
            return activity.ImagenTaskCompletionSource.Task;
        }
    }
}