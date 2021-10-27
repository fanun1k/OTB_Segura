using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;


namespace OTB_SEGURA.ViewModels
{
    class ViewCameraViewModel : BaseViewModel
    {

        #region Attributes
        private List<CameraModel> listaCamera;
        private INavigation Navigation;
        CameraService cameraService = new CameraService();

        #endregion

        #region Properties
        public List<CameraModel> ListaCamera
        {
            get { return listaCamera; }
            set { listaCamera = value; OnPropertyChanged(); }
            
        }

        #endregion

        #region Commands

        public ICommand ItemTappedCommand
        {
            get
            {
                return new RelayCommand(async() => {
                    try
                    {
                       await  Navigation.PushAsync(new View_SeeCamera());
                    }
                    catch (Exception ex)
                    {

                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                });
            }

        }
        public ICommand ApperingCommand
        {
            get
            {

                return new RelayCommand(async() =>
                {
                    int idOtb = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                    ResponseHTTP<CameraModel> respServer = await cameraService.GetCameraList(idOtb);
                    if (respServer.Code == System.Net.HttpStatusCode.OK)
                    {
                        ListaCamera = respServer.Data;
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert(respServer.Msj);
                    }
                });

            }

        }
        #endregion


        #region Constructor
        public ViewCameraViewModel(INavigation nav)
        {
            Title = "Ver Cámaras";
            Navigation = nav;
        }
        #endregion


    }
}
