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
        private async void InsertRoboMethod()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            var activity = new ActivityModel
            {
                Message = message,
                Type = "Robo",
                UserId = null,//no se utiliza por falta de manejo de sesiones a implementar con login de usuarios
                Latitude = position.Latitude,
                Longitude = position.Longitude,
                DateTime = DateTime.Now
            };

            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");
        }
        private async void InsertAccidenteMethod()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            var activity = new ActivityModel
            {
                Message = message,
                Type = "Accidente",
                UserId = null,//no se utiliza por falta de manejo de sesiones a implementar con login de usuarios
                Latitude = position.Latitude,
                Longitude = position.Longitude,
                DateTime = DateTime.Now
            };

            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");
        }
        private async void InsertIncendioMethod()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            var activity = new ActivityModel
            {
                Message = message,
                Type = "Incendio",
                UserId = null,//no se utiliza por falta de manejo de sesiones a implementar con login de usuarios
                Latitude = position.Latitude,
                Longitude = position.Longitude,
                DateTime = DateTime.Now
            };

            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");
        }
        private async void InsertDesastreMethod()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            var activity = new ActivityModel
            {
                Message = message,
                Type = "Desastre",
                UserId = null,//no se utiliza por falta de manejo de sesiones a implementar con login de usuarios
                Latitude = position.Latitude,
                Longitude = position.Longitude,
                DateTime = DateTime.Now
            };

            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Actividad agregada con exito");
        }
        private async void EmergencyMethod()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            var activity = new ActivityModel
            {
                Type = "Emergencia",
                UserId = null,//no se utiliza por falta de manejo de sesiones a implementar con login de usuarios
                Latitude = position.Latitude,
                Longitude = position.Longitude,
                DateTime = DateTime.Now
            };
            await fireBaseHelper.AddActivity(activity);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Actividad de Emergencia Guardada");
        }
        #endregion
    }
}
