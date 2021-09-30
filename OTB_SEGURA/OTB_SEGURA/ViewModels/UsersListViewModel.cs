using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    /// <summary>
    /// clase UserListViewModel que nos sirbe de contexto de datos de la vista View_UserList
    /// la clase contiene la logica de la vista View_UserList
    /// </summary>
    public class UsersListViewModel : BaseViewModel
    {

        #region Attributes
        public INavigation Navigation { get; set; }
        private UserModel userSelected = new UserModel();
        private UserService userService = new UserService();
        private List<UserModel> userList;
        #endregion

        #region Properties
        public UserModel UserSelected
        {
            get { return userSelected; }
            set { userSelected = value; OnPropertyChanged(); }
        }
        public List<UserModel> UserList
        {
            get { return userList; }
            set
            {
                userList = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Construct
        /// <summary>
        /// Constructor de la clase que recibe un parametro del tipo INavigation
        /// Este constructor tambien ejecuta la inicializacion del comando ItemTappedCommand
        /// </summary>
        /// <param name="navigation"> tipo INavigacion esto para poder hacer redirecciones a otras vistas de interfaz</param>
        public UsersListViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Lista de Usuarios";
            //InitCommandTapped();
        }
        #endregion

        #region Commands   
        /// <summary>
        ///  comando que se ejecuta cuando el usuario hace click en un item del listview de la vista
        ///  el comando necesita inicializarse con el metodo InitCommandTapped
        /// </summary>
        public ICommand ItemTappedCommand
        {
            get
            {
                return new RelayCommand(async() =>
                {
                    try
                    {
                        DependencyService.Get<IMessage>().LongAlert(userSelected.Name);
                        await Navigation.PushAsync(new View_UserProfile(userSelected));
                    }
                    catch (System.Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }

                });
            }
        }

        /// <summary>
        /// comando que se ejecuta cuando aparece la vista View_UserList en pantalla
        /// </summary>
        public ICommand AppearingCommand
        {
            get
            {
                return new RelayCommand(LoadData);
            }
        }
        /// <summary>
        /// comando que se ejecuta cuando se refresca el listview de la interfaz
        /// </summary>
        public ICommand RefreshingCommand
        {
            get
            {
                return new RelayCommand(LoadData);
            }
        }
        #endregion

        #region Method

        /// <summary>
        /// Metodo LoadData que nos sirbe para cargar los usuarios de la base de datos Firebase 
        /// este metodo pone un color al item de usuario dependiendo si su estado es activo o inactivo
        /// </summary>
        public async void LoadData()
        {
            try
            {
                int otbID = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                ResponseHTTP<UserModel> responseHTTP = await userService.UsersByOtb(otbID);
                if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                {
                    UserList = responseHTTP.Data;
                    //--Agregar video 2
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                }
            }
            catch (System.Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }

        }
        #endregion
    }
}
