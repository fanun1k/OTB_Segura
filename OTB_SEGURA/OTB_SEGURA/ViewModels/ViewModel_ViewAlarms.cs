using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    class ViewModel_ViewAlarms:BaseViewModel
    {
        

        #region Attributes
        private List<AlarmModel> listAlarm;
        AlarmService alarmService = new AlarmService();
        private INavigation Navigation { get; set; }
        #endregion

        #region Properties
        public List<AlarmModel> ListAlarm
        {
            get { return listAlarm; }
            set { listAlarm = value; OnPropertyChanged(); }

        }

        #endregion
        #region Commands

        public ICommand SeeAlarms
        {
            get
            {
                return new RelayCommand(() => DependencyService.Get<IMessage>().LongAlert("Ver alarma"));

            }

        }
        public ICommand ApperingCommandAlarm
        {
            get
            {

                return new RelayCommand(async () =>
                {
                    int idOtb = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                    ResponseHTTP<AlarmModel> respServer = await alarmService.GetAlarmList(idOtb);
                    if (respServer.Code == System.Net.HttpStatusCode.OK)
                    {
                        ListAlarm = respServer.Data;
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert(respServer.Msj);
                    }
                });

            }

        }
        //public ICommand ItemTappedCommand
        //{
        //    get
        //    {

        //        return new RelayCommand(async () =>
        //        {
        //            try
        //            {
        //                //Navigation.PushAsync(new);
        //                //TO-DO crear una interfaz para detonar las alarmas
        //            }
        //            catch (Exception ex)
        //            {

        //                DependencyService.Get<IMessage>().LongAlert(ex.Message);
        //            }
        //        });

        //    }

        //}
        #endregion

        #region Constructor
        public ViewModel_ViewAlarms(INavigation nav)
        {
            Title = "Ver Alarmas";
            Navigation = nav;
        }
        #endregion
    }
}
