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
        FireBaseHelper fireBaseHelper = new FireBaseHelper(); //instancia de helper de BDD
        #region Attributes 
        private string userId;//identificador de usuario
        private double latitude;//latitud de ubicacion
        private double longitude;//longitud de ubicacion
        private DateTime dateTimeAttribute;//fecha y hora de emergencia
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
        public AddEmergencyViewModel()//constructor de la clase
        {
            Title = "Lanzar Alarma de Emergencia"; //Titulo de la vista
        }
        #region Command
        public ICommand EmergencyCommand //comando de boton
        {
            get
            {
                return new RelayCommand(EmergencyMethod);//referencia de metodo de creacion de la emergencia
            }
        }
        #endregion
        #region Method
        private async void EmergencyMethod()//metodo de inicializacion de la emergencia
        {
            getLocation();//llamada a metodo para obtener ubicacion
            await Task.Delay(1000);
            var emergency = newEmergency();//Generar la emergencia
            await fireBaseHelper.AddEmergency(emergency);//llamada al metodo del helper para insertar la emergencia
            await Task.Delay(1000);
        }
        private EmergencyModel newEmergency()//metodo de creacion de la emergencia
        {
            return new EmergencyModel //creando el objeto de la emergencia
            {
                UserId = Application.Current.Properties["Id"].ToString(),//obtener id del usuario
                Latitude = Latitude,//obteniendo valores de latitud de la ubicacion
                Longitude = Longitude,//obteniendo valores de latitud de la ubicacion
                DateTime = DateTime.Now//obteniendo fecha y hora actual
            };
        }
        private async void getLocation()//metodo para obtener la ubicacion
        {
            var locator = CrossGeolocator.Current;//Nueva instancia para obtener ubicacion
            locator.DesiredAccuracy = 50;//definiendo la precision de la ubicacion
            var position = await locator.GetPositionAsync();//metodo para obtener la ubicacion
            latitude = position.Latitude;//asignando valores de latitud a variables globales
            longitude = position.Longitude;//asignando valores de longitud a variables globales
        }
        #endregion
    }
}
