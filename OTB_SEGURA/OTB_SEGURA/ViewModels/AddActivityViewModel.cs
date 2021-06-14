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

namespace OTB_SEGURA.ViewModels
{
    class AddActivityViewModel : BaseViewModel
    {
        FireBaseHelper fireBaseHelper = new FireBaseHelper();
        #region Attributes

        private string message;
        private string type;
        private string userId; //no se utiliza por falta de manejo de sesiones a implementar con login de usuarios
        private double latitude;
        private double longitude;
        private DateTime dateTimeAttribute;

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
            Title = "Agregar Nueva Actividad";
        }
        #region Command
        public ICommand InsertRoboCommand
        {
            get
            {
                return new RelayCommand(InsertRoboMethod);
            }
        }
        public ICommand InsertAccidenteCommand
        {
            get
            {
                return new RelayCommand(InsertAccidenteMethod);
            }
        }
        public ICommand InsertIncendioCommand
        {
            get
            {
                return new RelayCommand(InsertIncendioMethod);
            }
        }
        public ICommand InsertDesastreCommand
        {
            get
            {
                return new RelayCommand(InsertDesastreMethod);
            }
        }
        public ICommand EmergencyCommand
        {
            get
            {
                return new RelayCommand(EmergencyMethod);
            }
        }
        #endregion

        #region Method
        private async void getLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            latitude = position.Latitude;
            longitude = position.Longitude;
        }
        private ActivityModel newActivity(string message,string type) {
            return new ActivityModel
            {
                Message = message,
                Type = type,
                UserId = Application.Current.Properties["Id"].ToString(),
                Latitude = Latitude,
                Longitude = Longitude,
                DateTime = DateTime.Now
            };
        }
        private async void PostNotification(ActivityModel activity) {
            try
            {
                var serverKey = string.Format("key={0}", "AAAA4qfqqWY:APA91bGsZ0rQCczErCiSv-8-1bpO_U_Jmp1Q7-_GckSTf44jWe8MGKxQd0eJqb3IXXRJqQFsTaIW2POYZ_rjVrZrx492PmIyDGFjWaF3rmJ94IUVxRuce6KPf_TdTDngg7f0Hy35PWV_");
                var senderId = string.Format("id={0}", "973479782758");
                var data = new
                {
                    to = "/topics/all",
                    notification = new
                    {
                        body = activity.Message,
                        title = activity.Type
                    }

                };
                var jsonBody = JsonConvert.SerializeObject(data);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (!result.IsSuccessStatusCode)
                        {
                            DependencyService.Get<IMessage>().LongAlert("No se Pudo Notificar");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("Error: "+ex.Message);
            }
        }
        private async void InsertRoboMethod()
        {
            getLocation();
            await Task.Delay(1000);
            var activity = newActivity(message,"Robo");
            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito"+activity.UserId);
            PostNotification(activity);
        }
        private async void InsertAccidenteMethod()
        {
            getLocation();
            await Task.Delay(1000);
            var activity = newActivity(message, "Accidente");
            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");
            PostNotification(activity);
        }
        private async void InsertIncendioMethod()
        {
            getLocation();
            await Task.Delay(1000);
            var activity = newActivity(message, "Incendio");
            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");
            PostNotification(activity);
        }
        private async void InsertDesastreMethod()
        {
            getLocation();
            await Task.Delay(1000);
            var activity = newActivity(message, "Desastre");
            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");
            PostNotification(activity);
        }
        private async void EmergencyMethod()
        {
            getLocation();
            await Task.Delay(1000);
            var activity = newActivity("Alerta de Emergncia iniciada", "Emergencia");
            var emergency = newEmergency(activity);
            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            await fireBaseHelper.AddEmergency(emergency);
            await Task.Delay(1000);
            PostNotification(activity);
        }
        private EmergencyModel newEmergency(ActivityModel activity)
        {
            return new EmergencyModel
            {
                UserId = activity.UserId,
                Latitude = activity.Latitude,
                Longitude = activity.Longitude,
                DateTime = activity.DateTime
            };
        }
        #endregion
    }
}
