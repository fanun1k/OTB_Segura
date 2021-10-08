using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using OTB_SEGURA.Views;
using OTB_SEGURA.Services;
using OTB_SEGURA.Models;

namespace OTB_SEGURA.ViewModels
{
     class MyOtbViewModel:BaseViewModel
    {
        private OtbService otbService = new OtbService();
        private string myOTBName;


        public string MyOTBName
        {
            get { return myOTBName; }
            set { myOTBName = value; OnPropertyChanged(); }
        }

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
                    await Navigation.PushAsync(new View_ViewCamera());
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

        public  ICommand AppearingCommand
        {
            get
            {
                return new RelayCommand( async () =>
                {
                    try
                    {
                        ResponseHTTP<OtbModel> resultHTTP = await otbService.GetOtb(int.Parse(Application.Current.Properties["Otb_ID"].ToString()));
                        if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                        {
                            MyOTBName = resultHTTP.Data[0].Name;
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                });
            }
        }

        public MyOtbViewModel(INavigation nav)
        {
            Title = "Mi OTB";
            Navigation = nav;
        }
    }
}
