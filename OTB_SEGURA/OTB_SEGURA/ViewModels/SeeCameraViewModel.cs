using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    class SeeCameraViewModel : BaseViewModel
    {
        private ICommand seeCamera;

        public ICommand SeeCamera
        {
            get
            {
                return new RelayCommand(() => DependencyService.Get<IMessage>().LongAlert("Esta funcionando"));

            }

        }




        public SeeCameraViewModel()
        {
            Title = "Ver Cámaras";
        }
    }
}
