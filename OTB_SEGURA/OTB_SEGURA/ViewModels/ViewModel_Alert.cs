using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
     class ViewModel_Alert:BaseViewModel
    {
        AlertService alertService = new AlertService();
        public ViewModel_Alert( string title)
        {
            Title = title;
            LoadData();

        }

        #region Attributes
        private List<AlertModel> listAlert= new List<AlertModel>(); // instancia de la lista de actividades 
        #endregion

        #region Properties
        public List<AlertModel> ListAlert
        {
            get { return listAlert; }
            set
            {
                listAlert = value;
                OnPropertyChanged();
            }
            
        }
        private List <AlertModel> listaAlertas;

        public List <AlertModel> ListaAlertas
        {
            get { return listaAlertas; }
            set {  listaAlertas = value; OnPropertyChanged(); }
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
        public async void LoadData()
        {
            try
            {
                ResponseHTTP<AlertModel> response = await alertService.listarAlertas(int.Parse(Application.Current.Properties["Otb_ID"].ToString()));
                if (response.Code == System.Net.HttpStatusCode.OK)
                {
                    listaAlertas = response.Data;
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert(response.Msj);
                }
                //List<AlertModel> listaenviar = new List<AlertModel>();
                //switch (Title)
                //{
                //    case "General":
                //        ListAlert = listaAlertas;
                //        break;
                //    case "Robos":
                //        ListAlert = SepararPorAlertas(listaAlertas, 1);
                //        break;
                //    case "Incendios":
                //        ListAlert = SepararPorAlertas(listaAlertas, 2);
                //        break;
                //    case "Accidentes":
                //        ListAlert = SepararPorAlertas(listaAlertas, 3);
                //        break;
                //    case "Rescates":
                //        ListAlert = SepararPorAlertas(listaAlertas, 4);
                //        break;
                //}
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
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