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
using System.Linq;
using System.Collections.ObjectModel;

namespace OTB_SEGURA.ViewModels
{
    public class UserActivityViewModel:BaseViewModel
    {
        #region prop
        private List<AlertModel> listActivity=new List<AlertModel>(); // instancia de la lista de actividades 
        private AlertService alertService = new AlertService();
        private UserService userService = new UserService();
        private AlertTypeService alertTypeService = new AlertTypeService();
        private List<UserModel> userLis = new List<UserModel>();
        private List<AlertTypeModel> alertTypeList = new List<AlertTypeModel>();
        private ObservableCollection<CompleteAlertModel> listToShow;

        public ObservableCollection<CompleteAlertModel> ListToShow
        {
            get { return listToShow; }
            set { listToShow = value; OnPropertyChanged(); }
        }


        #endregion
        #region Construct
        public UserActivityViewModel()
        {
            Title = "Actividad de Usuarios"; // Titulo de la vista
        }
        #endregion

        #region Commands

        // Comando que carga los datos una vez se abre la vista
        public ICommand AppearingCommand
        {
            get
            {
                return new RelayCommand(async () => await LoadData()); ;
            }
        }

        //Comando que refresca los datos
        public ICommand RefreshingCommandActivityUsers
        {
            get
            {
                return new RelayCommand(async () => await LoadData()); ;
            }
        }

        // Comando que redirecciona a Google Maps con la locaclizacion de la actividad
        public ICommand ItemTappedCommandUserActivity { get; } = new Command(async (Item) =>
        {
            try
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
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }

        });
        #endregion

        #region Metodh

        // Metodo que carga la data de actividades de usuarios
        public async Task LoadData()
        {
            try
            {
               
                var tasks = new List<Task>();
                tasks.Add(LoadAlerts());
                tasks.Add(LoadAlertTypes());
                tasks.Add(LoadUsers());
                await Task.WhenAll(tasks);
                ListToShow = null;
                ListToShow = new ObservableCollection<CompleteAlertModel>();
                var query = from x in listActivity
                            select new CompleteAlertModel { Alert_ID = x.Alert_ID,
                                                            Alert_type_Name = alertTypeList.Where(y => y.Alert_type_ID == x.Alert_type_ID).Select(z => z.Name).FirstOrDefault(),
                                                            User_Name = userLis.Where(y => y.User_ID == x.User_ID).Select(z => z.Name).FirstOrDefault(),
                                                            Longitude=x.Longitude,
                                                            Latitude=x.Latitude,
                                                            Date=x.Date,
                                                            Message=x.Message                                                                                                                      
                            };
                foreach (var item in query)
                {
                    ListToShow.Add(item);
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }

        }

        private async Task LoadAlerts()
        {
            try
            {
                int otbId = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                ResponseHTTP<AlertModel> responseHTTP = await alertService.listarAlertas(otbId);
                listActivity = responseHTTP.Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
        private async Task LoadAlertTypes()
        {
            try
            {
                ResponseHTTP<AlertTypeModel> responseHTTP = await alertTypeService.GetAlertTypes();
                alertTypeList = responseHTTP.Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private async Task LoadUsers()
        {
            try
            {
                int otbId = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                ResponseHTTP<UserModel> responseHTTP = await userService.UsersByOtb(otbId);
                userLis = responseHTTP.Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }

}
