using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
     class ViewModel_AlertType:BaseViewModel
    {
        public ViewModel_AlertType( string title)
        {
            Title = title;
            LoadData();

        }

        #region prop
        private List<AlertModel> listAlert= new List<AlertModel>(); // instancia de la lista de actividades 

        #endregion

        #region Atributes
        public List<AlertModel> ListAlert
        {
            get { return listAlert; }
            set
            {
                listAlert = value;
                OnPropertyChanged();
            }
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
            var alertModel = Item as AlertModel; // Instancia del UserActivityViewModel
            if (alertModel != null)
            {
                await Map.OpenAsync(alertModel.Latitude, alertModel.Longitude, new MapLaunchOptions
                {
                    Name = "Ubicación",
                    NavigationMode = NavigationMode.None
                }); // Redireccion a Maps con la latitud y longitud
            }
        });
        #endregion

        #region Metodh

        // Metodo que carga la data de actividades de usuarios
        public void LoadData()
        {
            List<AlertModel> listaAlertas = new List<AlertModel>();
            listaAlertas.Add(new AlertModel
            {
                Alert_ID = 1,
                Date = DateTime.Now,
                State = 1,
                Alert_type_ID = 1,
                Longitude = (float)-17.38583,
                Latitude = (float)-66.18583,
                Otb_ID = 1,
                User_ID = 1

            });
            listaAlertas.Add(new AlertModel
            {
                Alert_ID = 1,
                Date = DateTime.Now,
                State = 1,
                Alert_type_ID = 2,
                Longitude = (float)-17.38583,
                Latitude = (float)-66.18583,
                Otb_ID = 1,
                User_ID = 2

            });
            listaAlertas.Add(new AlertModel
            {
                Alert_ID = 2,
                Date = DateTime.Now,
                State = 1,
                Alert_type_ID = 2,
                Longitude = (float)-17.38583,
                Latitude = (float)-66.18583,
                Otb_ID = 1,
                User_ID = 2

            });
            listaAlertas.Add(new AlertModel
            {
                Alert_ID = 3,
                Date = DateTime.Now,
                State = 1,
                Alert_type_ID = 3,
                Longitude = (float)-17.38583,
                Latitude = (float)-66.18583,
                Otb_ID = 1,
                User_ID = 2

            });
            listaAlertas.Add(new AlertModel
            {
                Alert_ID = 4,
                Date = DateTime.Now,
                State = 1,
                Alert_type_ID = 4,
                Longitude = (float)-17.38583,
                Latitude = (float)-66.18583,
                Otb_ID = 1,
                User_ID = 2

            });
            listaAlertas.Add(new AlertModel
            {
                Alert_ID = 5,
                Date = DateTime.Now,
                State = 1,
                Alert_type_ID = 1,
                Longitude = (float)-17.38583,
                Latitude = (float)-66.18583,
                Otb_ID = 1,
                User_ID = 2

            });

            List<AlertModel> listaenviar = new List<AlertModel>();
            switch (Title)
            {
                case "General":
                    ListAlert = listaAlertas;
                    break;
                case "Robos":
                    ListAlert = SepararPorAlertas(listaAlertas, 1);
                    break;
                case "Incendios":
                    ListAlert = SepararPorAlertas(listaAlertas, 2);
                    break;
                case "Accidentes":
                    ListAlert = SepararPorAlertas(listaAlertas, 3);
                    break;
                case "Rescates":
                    ListAlert = SepararPorAlertas(listaAlertas, 4);
                    break;

            }
            
            //    ListActivity = await firebaseHelper.GetAllActivities(); // Llamada al metodo del helper para obtener la data
        }

        private List<AlertModel> SepararPorAlertas(List<AlertModel> list, int type)
        {
            List<AlertModel> listDevolver = new List<AlertModel>();
            int count = 0;
            foreach ( var item in list)
            {
                if (item.Alert_type_ID==type)
                {
                    listDevolver.Add(list[count]);
                }
                count++;
            }
            return listDevolver;
        }
        #endregion

    }
}