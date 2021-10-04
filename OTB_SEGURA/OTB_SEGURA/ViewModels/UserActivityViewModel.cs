using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
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
        private ObservableCollection<CompleteAlertModel> listToShow = new ObservableCollection<CompleteAlertModel>();
        private AlertTypeModel alertTypeSelected;
        private bool group;
        private int indexPick;
        private CompleteAlertModel alertSelected;
        private INavigation Navigation;
        #endregion

        #region Properties
        public CompleteAlertModel AlertSelected
        {
            get { return alertSelected; }
            set { alertSelected = value; }
        }
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
        public UserActivityViewModel( INavigation nav)
        {
            Title = "Actividad de Usuarios"; // Titulo de la vista
            Navigation = nav;
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
        public ICommand ItemTappedCommandUserActivity
        {
            get
            {
                return new RelayCommand(async () =>
                {

                    if (alertSelected!=null)
                    {
                        await Navigation.PushAsync(new View_Maps(alertSelected));                        
                    }
                });
            }
        }
        public ICommand SelectedChangedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        if (alertTypeSelected != null) //Filtrado
                        {
                            if (!group)//filtrado sin agrupado
                            {
                                await FilterList();
                            }
                            else //filtrado y agrupado
                            {
                                await FilterAndGroup();
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
                        if (group)//check en true
                        {
                            if (alertTypeSelected != null)// agrupado y filtrado
                            {
                                await FilterAndGroup();
                            }
                            else//solo agrupado
                            {
                                await GroupList();
                            }
                        }
                        else//check en false
                        {
                            if (alertTypeSelected != null)//filtrado
                            {
                                await FilterList();
                            }
                            else await LoadData(); // sin agrupar y sin filtrar
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
                await ClearList();
                IndexPick = -1;
                Group = false;
                var query = from x in listActivity
                            orderby x.Date descending
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
        private async Task FilterAndGroup()
        {
            try
            {
                await ClearList();
                await Task.Run(() =>
                {
                    var query = from x in alertTypeList
                                where x.Alert_type_ID == alertTypeSelected.Alert_type_ID
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

                });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private async Task ClearList()
        {
            await Task.Run(() =>
            {
                ListToShow.Clear();
            });
        }
        private async Task FilterList()
        {
            try
            {
                if (alertTypeSelected != null)
                {
                    await ClearList();
                    await Task.Run(() =>
                    {
                        var query = from x in listActivity
                                    where x.Alert_type_ID == alertTypeSelected.Alert_type_ID
                                    orderby x.Date descending
                                    select new CompleteAlertModel
                                    {
                                        Alert_ID = x.Alert_ID,
                                        Alert_type_Name = AlertTypeList.Where(y => y.Alert_type_ID == x.Alert_type_ID).Select(z => z.Name).FirstOrDefault(),
                                        User_Name = userLis.Where(y => y.User_ID == x.User_ID).
                                        Select(z => z.Name).FirstOrDefault(),
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
                await ClearList();
                await Task.Run(() =>
                {
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
                });

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
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    ResponseHTTP<AlertModel> responseHTTP = await alertService.listarAlertas(otbId);
                    if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                    {
                        listActivity = responseHTTP.Data;
                        await App.SQLiteDB.SaveAlertAsync(listActivity);
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                    }
                }
                else
                {
                    listActivity = await App.SQLiteDB.GetAlertAsync();
                }
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
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    ResponseHTTP<AlertTypeModel> responseHTTP = await alertTypeService.GetAlertTypes();
                    if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                    {
                        AlertTypeList = new ObservableCollection<AlertTypeModel>(responseHTTP.Data);
                        await App.SQLiteDB.SaveAlertTypeAsync(responseHTTP.Data);
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                    }
                }
                else
                {
                    var aux = await App.SQLiteDB.GetAlertTypeAsync();
                    AlertTypeList = new ObservableCollection<AlertTypeModel>(aux);
                }
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
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    ResponseHTTP<UserModel> responseHTTP = await userService.UsersByOtb(otbId);
                    if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                    {
                        userLis = responseHTTP.Data;
                        await App.SQLiteDB.SaveUserAsync(userLis);
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                    }
                }
                else
                {
                    userLis = await App.SQLiteDB.GetUserAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

}
