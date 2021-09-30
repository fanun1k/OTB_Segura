using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    class AddAlertViewModel : BaseViewModel
    {
        #region Attributes
        private AlertTypeService alertTypeService = new AlertTypeService();
        public List<AlertTypeModel> listAlertsType;
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
        public AddAlertViewModel()
        {
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
                        else
                        {
                            DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
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
                        if (await App.Current.MainPage.DisplayAlert("Eliminar Tipo de Alerta","¿Desea eliminar el tipo de alerta?", "Aceptar", "Cancelar"))
                        {
                            responseHTTP = await alertTypeService.DeleteAlertType(selectedAlert.Alert_type_ID);
                            if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                            {
                                await LoadAlertsType();
                            }
                            DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }

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
                    ListAlertsType = responseHTTP.Data;
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
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
