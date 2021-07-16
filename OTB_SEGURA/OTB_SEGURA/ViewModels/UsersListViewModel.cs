using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
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
        FireBaseHelper firebaseHelper = new FireBaseHelper();
        public INavigation Navigation { get; set; }
        private List<UserModel> userList;
        #endregion

        #region Properties
        public List<UserModel> UserList
        {
            get { return userList; }
            set { userList = value;
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
            InitCommandTapped();
        }
        #endregion

        #region Commands   
        /// <summary>
        ///  comando que se ejecuta cuando el usuario hace click en un item del listview de la vista
        ///  el comando necesita inicializarse con el metodo InitCommandTapped
        /// </summary>
        public ICommand ItemTappedCommand { get; protected set; }

        /// <summary>
        /// comando que se ejecuta cuando aparece la vista View_UserList en pantalla
        /// </summary>
        public ICommand AppearingCommand
        {
            get
            {
                return  new RelayCommand(LoadData);
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
            UserList = await firebaseHelper.GetAllUsers();
            foreach (var user in userList)
            {
                if (user.State == 0)
                {
                    user.StateColor = "#ff4e4e";
                    user.StateBorderColor = "#ff4e4e";
                }
                else
                {
                    user.StateBorderColor = "#056d8a";
                }
            }
        }
        /// <summary>
        /// Metodo que que inicia el comando ItemTappedCommnad
        /// </summary>
        public void InitCommandTapped()
        {
            ItemTappedCommand = new Command(async (item) => {
                var user = item as UserModel;
                await Navigation.PushAsync(new View_UserProfile(user));
            });
        }
        #endregion
    }
}
