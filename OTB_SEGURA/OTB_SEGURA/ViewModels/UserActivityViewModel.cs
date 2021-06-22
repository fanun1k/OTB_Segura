using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    public class UserActivityViewModel:BaseViewModel
    {
        #region prop
        FireBaseHelper firebaseHelper = new FireBaseHelper(); // instancia de helper de BDD
        private List<UserActivityModel> listActivity=new List<UserActivityModel>(); // instancia de la lista de actividades 

        #endregion

        public List<UserActivityModel> ListActivity
        {
            get { return listActivity; }
            set { listActivity = value;
                OnPropertyChanged();
            }
        }

        #region Construct
        public UserActivityViewModel()
        {
            Title = "Actividad de Usuarios"; // Titulo de la vista
            LoadData(); // Carga de los datos
        }
        #endregion

        #region Commands

        // Comando que carga los datos una vez se abre la vista
        public ICommand AppearingCommand
        {
            get
            {
                return new RelayCommand(LoadData);
            }
        }

        //Comando que refresca los datos
        public ICommand RefreshingCommandActivityUsers
        {
            get
            {
                return new RelayCommand(LoadData);
            }
        }

        // Comando que redirecciona a Google Maps con la locaclizacion de la actividad
        public ICommand ItemTappedCommandUserActivity { get; } = new Command(async (Item) =>
        {
            var userActivityModel = Item as UserActivityModel; // Instancia del UserActivityViewModel
            if (userActivityModel != null)
            {
                await Map.OpenAsync(userActivityModel.Latitude, userActivityModel.Longitude, new MapLaunchOptions
                {
                    Name = "Ubicación",
                    NavigationMode = NavigationMode.None
                }); // Redireccion a Maps con la latitud y longitud
            }
        });
        #endregion

        #region Metodh

        // Metodo que carga la data de actividades de usuarios
        public async void LoadData()
        {
            ListActivity = await firebaseHelper.GetAllActivities(); // Llamada al metodo del helper para obtener la data
        }
        #endregion
    }

}
