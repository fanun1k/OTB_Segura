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
    class AddActivityViewModel : BaseViewModel
    {
        FireBaseHelper fireBaseHelper = new FireBaseHelper();//instancia de helper de BDD
        #region Attributes

        private string message="";//mensaje de la actividad
        private string type;//tipo de la actividad
        private string userId;//identificador de usuario
        private double latitude;//latitud de ubicacion
        private double longitude;//longitud de ubicacion
        private DateTime dateTimeAttribute;//fecha y hora de emergencia

        #endregion
        #region Properties
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
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
        public AddActivityViewModel()
        {
            Title = "Agregar Nueva Actividad";//Titulo de la vista
        }
        #region Command
        public ICommand InsertRoboCommand//comando de boton de robo
        {
            get
            {
                return new RelayCommand(InsertRoboMethod);//referencia de metodo de creacion de la actividad de robo
            }
        }
        public ICommand InsertAccidenteCommand//comando de boton de accidente
        {
            get
            {
                return new RelayCommand(InsertAccidenteMethod);//referencia de metodo de creacion de la actividad de accidente
            }
        }
        public ICommand InsertIncendioCommand//comando de boton de incendio
        {
            get
            {
                return new RelayCommand(InsertIncendioMethod);//referencia de metodo de creacion de la actividad de incendio
            }
        }
        public ICommand InsertDesastreCommand//comando de boton de desastre
        {
            get
            {
                return new RelayCommand(InsertDesastreMethod);//referencia de metodo de creacion de la actividad de desastre
            }
        }
        public ICommand EmergencyCommand//comando de boton de emergencia
        {
            get
            {
                return new RelayCommand(EmergencyMethod);//referencia de metodo de creacion de la emergencia
            }
        }
        #endregion

        #region Method
        private async void getLocation()//metodo para obtener la ubicacion
        {
            var locator = CrossGeolocator.Current;//Nueva instancia para obtener ubicacion
            locator.DesiredAccuracy = 50;//definiendo la precision de la ubicacion
            var position = await locator.GetPositionAsync();//metodo para obtener la ubicacion
            latitude = position.Latitude;//asignando valores de latitud a variables globales
            longitude = position.Longitude;//asignando valores de longitud a variables globales
        }
        private ActivityModel newActivity(string message,string type)//metodo de creacion de la actividad
        {
            return new ActivityModel//creando el objeto de la emergencia
            {
                Message = message,//definiendo mensaje de la actividad
                Type = type,//definiendo tipo de la actividad
                UserId = Application.Current.Properties["Id"].ToString(),//obtener id del usuario
                Latitude = Latitude,//obteniendo valores de latitud de la ubicacion
                Longitude = Longitude,//obteniendo valores de latitud de la ubicacion
                DateTime = DateTime.Now//obteniendo fecha y hora actual
            };
        }
        private async void PostNotification(ActivityModel activity) {//metodo de envio de notificaiciones
            try
            {
                //llave del servidor para peticiones http/post para generar notificaciones
                var serverKey = string.Format("key={0}", "AAAA4qfqqWY:APA91bGsZ0rQCczErCiSv-8-1bpO_U_Jmp1Q7-_GckSTf44jWe8MGKxQd0eJqb3IXXRJqQFsTaIW2POYZ_rjVrZrx492PmIyDGFjWaF3rmJ94IUVxRuce6KPf_TdTDngg7f0Hy35PWV_");
                var senderId = string.Format("id={0}", "973479782758");//id del sender da las notificaciones
                var data = new //datos de la notificaciones
                {
                    to = "/topics/all",//topic el que se mandara la notificacion
                    notification = new//nueva notificacion 
                    {
                        body = activity.Message,//mensaje de la notificacion
                        title = activity.Type//titulo de la notificacion
                    }

                };
                var jsonBody = JsonConvert.SerializeObject(data);//generando json para las peticiones

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))//variable de envio de notificaciones
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);//definicion del header de autorizacion
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);//definicion del header del sender 
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");//definicion del contenido con json de la notificacion

                    using (var httpClient = new HttpClient())//variable de cliente de la peticion
                    {
                        var result = await httpClient.SendAsync(httpRequest);//definicion de variable de resultado de la peticion
                        if (!result.IsSuccessStatusCode)
                        {
                            DependencyService.Get<IMessage>().LongAlert("No se Pudo Notificar");//mensaje de error 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("Error: "+ex.Message);//mensaje en caso de error
            }
        }

        // Metodo de validacion de los Entry de la vista
        public bool ValidationEntry()
        {
            bool res=true;
            if (!message.Equals(""))
            {
                if (!Regex.Match(message, "^[ñA-Za-záéíóúÁÉÍÓÚ _]*[ñA-Za-záéíóúÁÉÍÓÚ][ñA-Za-záéíóúÁÉÍÓÚ _]*$").Success)
                {
                    res = false;
                    DependencyService.Get<IMessage>().LongAlert("Formato del mensaje incorrecto");
                }
            }
            else
            {
                res = false;
                DependencyService.Get<IMessage>().LongAlert("Por favor, introduzca una descripción del suceso");
            }

            return res;
        }

        private async void InsertRoboMethod()//insert del robo
        {
            IsBusy = true;
            if (ValidationEntry())
            {
                getLocation();//llamada a metodo para obtener ubicacion
                await Task.Delay(1000);
                var activity = newActivity(message, "Robo");//Generar la actividad tipo robo
                await fireBaseHelper.AddActivity(activity);//llamada al metodo del helper para insertar la actividad
                await Task.Delay(1000);
                DependencyService.Get<IMessage>().LongAlert("Actividad agregada con éxito");//Mensaje de exito de la insercion
                PostNotification(activity);//llamada al metodo del envio de la notificacion
            }
            IsBusy = false;
        }
        private async void InsertAccidenteMethod()
        {
            IsBusy = true;
            if (ValidationEntry())
            {
                getLocation();//llamada a metodo para obtener ubicacion
                await Task.Delay(1000);
                var activity = newActivity(message, "Accidente");//Generar la actividad tipo accidente
                await fireBaseHelper.AddActivity(activity);//llamada al metodo del helper para insertar la actividad
                await Task.Delay(1000);
                DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");//Mensaje de exito de la insercion
                PostNotification(activity);//llamada al metodo del envio de la notificacion
            }
            IsBusy = false;
        }
        private async void InsertIncendioMethod()
        {
            IsBusy = true;
            if (ValidationEntry())
            {
                getLocation();//llamada a metodo para obtener ubicacion
                await Task.Delay(1000);
                var activity = newActivity(message, "Incendio");//Generar la actividad tipo incendio
                await fireBaseHelper.AddActivity(activity);//llamada al metodo del helper para insertar la actividad
                await Task.Delay(1000);
                DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");//Mensaje de exito de la incendio
                PostNotification(activity);//llamada al metodo del envio de la notificacion
            }
            IsBusy = false;
        }
        private async void InsertDesastreMethod()
        {
            IsBusy = true;
            if (ValidationEntry())
            {
                getLocation();//llamada a metodo para obtener ubicacion
                await Task.Delay(1000);
                var activity = newActivity(message, "Desastre");//Generar la actividad tipo desastre
                await fireBaseHelper.AddActivity(activity);//llamada al metodo del helper para insertar la actividad
                await Task.Delay(1000);
                DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");//Mensaje de exito de la desastre
                PostNotification(activity);//llamada al metodo del envio de la notificacion
            }
            IsBusy = true;  
        }
        private async void EmergencyMethod()
        {
            getLocation();//llamada a metodo para obtener ubicacion
            await Task.Delay(1000);
            var activity = newActivity("Alerta de Emergncia iniciada", "Emergencia");//Generar la actividad tipo emergencia
            var emergency = newEmergency(activity);//llamada al metodo del helper para insertar la actividad tipo emergencia
            await fireBaseHelper.AddActivity(activity);//llamada al metodo del helper para insertar la actividad
            await Task.Delay(1000);
            await fireBaseHelper.AddEmergency(emergency);//llamada al metodo del helper para insertar la emergencia
            await Task.Delay(1000);
            PostNotification(activity);//llamada al metodo del envio de la notificacion
        }
        private EmergencyModel newEmergency(ActivityModel activity)
        {
            return new EmergencyModel
            {
                UserId = activity.UserId,//obtener id del usuario
                Latitude = activity.Latitude,//obteniendo valores de latitud de la ubicacion
                Longitude = activity.Longitude,//obteniendo valores de latitud de la ubicacion
                DateTime = activity.DateTime//obteniendo fecha y hora actual
            };
        }
        #endregion
    }
}
