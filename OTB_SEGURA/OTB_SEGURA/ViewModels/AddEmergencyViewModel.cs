using System;
using System.Collections.Generic;
using System.Text;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading;
using Plugin.Geolocator;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace OTB_SEGURA.ViewModels
{
    class AddEmergencyViewModel:BaseViewModel
    {
        FireBaseHelper fireBaseHelper = new FireBaseHelper();
        #region Attributes
        private string userId;
        private double latitude;
        private double longitude;
        private DateTime dateTimeAttribute;
        #endregion
        #region Properties
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        public DateTime DateTimeAttribute
        {
            get { return dateTimeAttribute; }
            set { dateTimeAttribute = value; }
        }

        #endregion
        public AddEmergencyViewModel()
        {
            Title = "Lanzar Alarma de Emergencia";
        }
        #region Command
        public ICommand EmergencyCommand
        {
            get
            {
                return new RelayCommand(EmergencyMethod);
            }
        }
        #endregion
        #region Method
        private async void EmergencyMethod()
        {
            getLocation();
            await Task.Delay(1000);
            var emergency = newEmergency();
            await fireBaseHelper.AddEmergency(emergency);
            await Task.Delay(1000);
        }
        private EmergencyModel newEmergency()
        {
            return new EmergencyModel
            {
                UserId = Application.Current.Properties["Id"].ToString(),
                Latitude = Latitude,
                Longitude = Longitude,
                DateTime = DateTime.Now
            };
        }
        private async void getLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            latitude = position.Latitude;
            longitude = position.Longitude;
        }
        #endregion
    }
}
