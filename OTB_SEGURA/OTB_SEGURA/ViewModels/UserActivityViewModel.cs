using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    public class UserActivityViewModel : BaseViewModel
    {
        #region Attributes
        private List<AlertModel> listActivity = new List<AlertModel>(); // instancia de la lista de actividades 
        private AlertService alertService = new AlertService();
        private UserService userService = new UserService();
        private AlertTypeService alertTypeService = new AlertTypeService();
        private List<UserModel> userLis = new List<UserModel>();
        private ObservableCollection<AlertTypeModel> alertTypeList = new ObservableCollection<AlertTypeModel>();
        private ObservableCollection<CompleteAlertModel> listToShow;
        private AlertTypeModel alertTypeSelected;
        private bool group;
        private int indexPick;
        #endregion

        #region Properties


        public ObservableCollection<AlertTypeModel> AlertTypeList
        {
            get { return alertTypeList; }
            set { alertTypeList = value; OnPropertyChanged(); }
        }


        public int IndexPick
        {
            get { return indexPick; }
            set { indexPick = value; OnPropertyChanged(); }
        }

        public bool Group
        {
            get { return group; }
            set { group = value; OnPropertyChanged(); }
        }
        public AlertTypeModel AlertTypeSelected
        {
            get { return alertTypeSelected; }
            set { alertTypeSelected = value; OnPropertyChanged(); }
        }

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



        public ICommand SelectedChangedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        if (alertTypeSelected != null)
                        {
                            if (!group)
                            {
                                await Filter();
                            }
                            else
                            {

                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                });
            }
        }

        public ICommand CheckedChangedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        if (group)
                        {

                            await GroupList();
                        }
                        else if (alertTypeSelected != null)
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                });
            }
        }

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
                IndexPick = -1;
                Group = false;
                var query = from x in listActivity
                            select new CompleteAlertModel
                            {
                                Alert_ID = x.Alert_ID,
                                Alert_type_Name = AlertTypeList.Where(y => y.Alert_type_ID == x.Alert_type_ID).Select(z => z.Name).FirstOrDefault(),
                                User_Name = userLis.Where(y => y.User_ID == x.User_ID).Select(z => z.Name).FirstOrDefault(),
                                Longitude = x.Longitude,
                                Latitude = x.Latitude,
                                Date = x.Date,
                                Message = x.Message
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

        private async Task Filter()
        {
            try
            {
                if (alertTypeSelected != null)
                {
                    ListToShow = null;
                    ListToShow = new ObservableCollection<CompleteAlertModel>();
                    await Task.Run(()=> {
                        var query = from x in listActivity
                                    where x.Alert_type_ID == alertTypeSelected.Alert_type_ID
                                    select new CompleteAlertModel
                                    {
                                        Alert_ID = x.Alert_ID,
                                        Alert_type_Name = AlertTypeList.Where(y => y.Alert_type_ID == x.Alert_type_ID).Select(z => z.Name).FirstOrDefault(),
                                        User_Name = userLis.Where(y => y.User_ID == x.User_ID).Select(z => z.Name).FirstOrDefault(),
                                        Longitude = x.Longitude,
                                        Latitude = x.Latitude,
                                        Date = x.Date,
                                        Message = x.Message
                                    };
                        foreach (var item in query)
                        {
                            ListToShow.Add(item);
                        }
                    });            
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task GroupList()
        {
            try
            {
                ListToShow = null;
                ListToShow = new ObservableCollection<CompleteAlertModel>();
                var query = from x in alertTypeList
                            select new CompleteAlertModel
                            {
                                Alert_type_Name = x.Name,
                                Ubication_List = listActivity.Where(y => y.Alert_type_ID == x.Alert_type_ID).
                                                              Select(z => new UbicationModel
                                                              {
                                                                  Latitude = z.Latitude,
                                                                  Longitude = z.Longitude
                                                              }).ToList()
                            };
                foreach (var item in query)
                {
                    ListToShow.Add(item);
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
                AlertTypeList = new ObservableCollection<AlertTypeModel>(responseHTTP.Data);
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
