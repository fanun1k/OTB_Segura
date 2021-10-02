using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using OTB_SEGURA.Views;


namespace OTB_SEGURA.ViewModels
{
     class MyOtbViewModel:BaseViewModel
    {
        private INavigation Navigation { get; set; }
        public ICommand RegisterCameraCommand
        {
            get
            {
                return new RelayCommand(Retorno);
            }
        }
        public async void Retorno()
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                scanner.TopText = "Scannea el código qr";
                scanner.BottomText = " Scanneando...";
                var result = await scanner.Scan();
                if (result != null)
                {
                    DependencyService.Get<IMessage>().LongAlert("Código scanneado");

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ICommand SeeCameraCommand
        {
            get
            {
                return new RelayCommand(async () => {
<<<<<<< HEAD
                    await Navigation.PushAsync(new View_SeeCamera());
=======
                    await Navigation.PushAsync(new View_ViewCamera());
>>>>>>> f606c0afb7540c3b2fd96d0abbc4d1be26d68df9
                });
            }
        }

        public ICommand RegisterAlarmCommand
        {
            get
            {
                return new RelayCommand(() =>
                    DependencyService.Get<IMessage>().LongAlert("Registrar Alarma")
                );
            }
        }
        public ICommand SeeAlarmCommand
        {
            get
            {
                return new RelayCommand(async()=> {
                    await Navigation.PushAsync(new View_ViewAlarms());
                });
            }
        }

        public ICommand AdministrateAlerts
        {
            get
            {
                return new RelayCommand(() =>
                    DependencyService.Get<IMessage>().LongAlert("Administrar Alertas")
                );
                
            }
        }

        public MyOtbViewModel(INavigation nav)
        {
            Title = "Mi OTB";
            Navigation = nav;
        }
    }
}
