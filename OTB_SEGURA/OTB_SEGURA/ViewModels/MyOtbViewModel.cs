using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;


namespace OTB_SEGURA.ViewModels
{
    class MyOtbViewModel:BaseViewModel
    {
        private ICommand registerCameraCommand;

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

        private ICommand seeCameraCommand;

        public ICommand SeeCameraCommand
        {
            get
            {
                return new RelayCommand(() =>
                    DependencyService.Get<IMessage>().LongAlert("Ver Camaras")
                );
            }
        }

        private ICommand registerAlarmCommand;

        public ICommand RegisterAlarmCommand
        {
            get
            {
                return new RelayCommand(() =>
                    DependencyService.Get<IMessage>().LongAlert("Registrar Alarma")
                );
            }
        }

        private ICommand seeAlarmCommand;

        public ICommand SeeAlarmCommand
        {
            get
            {
                return new RelayCommand(() =>
                    DependencyService.Get<IMessage>().LongAlert("Ver Alarmas")
                );
            }
        }

        public MyOtbViewModel()
        {
            Title = "Mi OTB";
        }
    }
}
