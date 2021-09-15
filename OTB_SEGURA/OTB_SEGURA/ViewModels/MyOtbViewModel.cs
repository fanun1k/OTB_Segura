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
                return new RelayCommand(() =>
                    DependencyService.Get<IMessage>().LongAlert("Registrar Camara")
                );
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
