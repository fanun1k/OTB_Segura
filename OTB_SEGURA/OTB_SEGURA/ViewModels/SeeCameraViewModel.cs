
using GalaSoft.MvvmLight.Command;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    class SeeCameraViewModel : BaseViewModel
    {
        private ImageSource imageVid;

        public ImageSource ImageVid
        {
            get { return imageVid; }
            set { imageVid = value;OnPropertyChanged(); }
        }



        public SeeCameraViewModel()
        {
            Title = "Ver Cámaras";
        }

        public ICommand SeeCamera
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        DependencyService.Get<IMessage>().LongAlert("PLAY");
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }              
                });
            
            }


        }
        public ImageSource stringToImage(string inputString)
        {
            byte[] imageBytes = Encoding.Unicode.GetBytes(inputString);

            // Don't need to use the constructor that takes the starting offset and length
            // as we're using the whole byte array.
            MemoryStream ms = new MemoryStream(imageBytes);

            ImageSource image = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));

            return image;
        }
    }
}
