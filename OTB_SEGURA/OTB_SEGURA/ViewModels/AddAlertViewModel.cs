using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    class AddAlertViewModel : BaseViewModel
    {
        #region Attributes
        private INavigation Navigation;
        private AlertTypeService alertTypeService = new AlertTypeService();
        public List<AlertTypeModel> listAlertsType = new List<AlertTypeModel>();
        private ResponseHTTP<AlertTypeModel> responseHTTP;
        private AlertTypeModel selectedAlert;
        private string alertName;
        #endregion
        #region Properties

        public List<AlertTypeModel> ListAlertsType
        {
            get { return listAlertsType; }
            set { listAlertsType = value; OnPropertyChanged(); }
        }

        public AlertTypeModel SelectedAlert
        {
            get { return selectedAlert; }
            set { selectedAlert = value; OnPropertyChanged(); }
        }

        public string AlertName
        {
            get { return alertName; }
            set { alertName = value; OnPropertyChanged(); }
        }

        #endregion
        #region Constructors
        public AddAlertViewModel(INavigation nav)
        {
            Navigation = nav;
        }
        #endregion

        #region Commands
        public ICommand AddAlertCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        responseHTTP = await alertTypeService.AddAlertType(alertName);
                        if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                        {
                            await LoadAlertsType();
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                });
            }
        }
        public ICommand AppearingCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        await LoadAlertsType();
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                });
            }
        }
        public ICommand RemoveAlertCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        if (await App.Current.MainPage.DisplayAlert("Eliminar Tipo de Alerta", "¿Desea eliminar el tipo de alerta?", "Aceptar", "Cancelar"))
                        {
                            responseHTTP = await alertTypeService.DeleteAlertType(selectedAlert.Alert_type_ID);
                            if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                            {
                                ListAlertsType = null;
                                await LoadAlertsType();
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }

                });
            }
        }
        public ICommand InfoCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await Navigation.PushAsync(new View_HelpAlert());
                });
            }
        }
        #endregion

        #region Methods

        private async Task LoadAlertsType()
        {
            try
            {
                responseHTTP = await alertTypeService.GetAlertTypes();
                if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                {
                    AlertName = "";
                    ListAlertsType = new List<AlertTypeModel>();
                    ListAlertsType = responseHTTP.Data;
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
        }


        #endregion
    }
}
