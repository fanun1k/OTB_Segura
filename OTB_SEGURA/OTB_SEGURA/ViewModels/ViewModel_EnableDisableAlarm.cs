using Firebase.Database;
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
    public class ViewModel_EnableDisableAlarm:BaseViewModel
    {
        #region Attributes
        FireBaseHelper firebase = new FireBaseHelper();
        #endregion

        public ViewModel_EnableDisableAlarm(AlarmModel alarma)
        {
            Title = alarma.Name;
        }
        #region Commands
        public ICommand EnableDisableAlarmCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        int res = await firebase.EnableDisableAlarm();
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
                return new RelayCommand(async()=> {
                    var firebase = new FirebaseClient("https://proyecto-emergencia-default-rtdb.firebaseio.com/");
                    var observable =  firebase
                      .Child("AlarmaPlaza")
                      .AsObservable<AlarmState>()
                      .Subscribe(d => DependencyService.Get<IMessage>().LongAlert(d.Key));
                });
            }
        }
        #endregion
    }
}
