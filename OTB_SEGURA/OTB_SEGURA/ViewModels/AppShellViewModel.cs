using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using OTB_SEGURA.Views;

namespace OTB_SEGURA.ViewModels
{
    /// <summary>
    /// Clase que nos sirbe como logica para la interfaz del menu hamburgueza AppShell
    /// </summary>
    public  class AppShellViewModel:BaseViewModel
    {
        List<String> listaMenu = new List<string>();
        
        #region Attributes
        AlertService alertService = new AlertService();
        private bool isAdmin=false;
        private bool isUser=false;
        private bool userWithOutOTB = false;
        private UserService userService = new UserService();
        public INavigation Navigation { get; set; }
        #endregion
        #region Properties
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; OnPropertyChanged(); }
        }
        public bool IsUser
        {
            get { return isUser; }
            set { isUser = value; OnPropertyChanged(); }
        }
        public bool UserWithOutOTB
        {
            get { return userWithOutOTB; }
            set { userWithOutOTB = value; OnPropertyChanged(); }
        }
        #endregion
        #region Construct
        /// <summary>
        /// Constructor que nos sirbe para determinar si el usuario que inicio sesion es administrador o usuario
        /// Esto oculta algunas caracteristicas de la vista
        /// </summary>
        public AppShellViewModel(INavigation nav)
        {
            Navigation = nav;
            if (Application.Current.Properties.ContainsKey("Sesion"))
            {
                switch (int.Parse(Application.Current.Properties["UserType"].ToString()))
                {
                    case 0:
                        IsAdmin = false;
                        IsUser = true;
                        UserWithOutOTB = false;
                        break;
                    case 1:
                        IsAdmin = true;
                        IsUser = true;
                        UserWithOutOTB = false;
                        break;
                    case 2:
                        IsAdmin = true;
                        IsUser = true;
                        UserWithOutOTB = false;
                        break;
                }
                if (Application.Current.Properties["Otb_ID"] == null)
                {
                    IsAdmin = false;
                    IsUser = false;
                    UserWithOutOTB = true;
                }
            }
            MessagingCenter.Subscribe<LoginViewModel>(this, "userWithOutOTB", (sender) => {
                IsAdmin = false;
                IsUser = false;
                UserWithOutOTB = true;
            });
            MessagingCenter.Subscribe<LoginViewModel>(this, "user", (sender) => {
                IsAdmin = false;
                IsUser = true;
                UserWithOutOTB = false;
            });
            MessagingCenter.Subscribe<LoginViewModel>(this, "admin", (sender) => {
                IsAdmin = true;
                IsUser = true;
                UserWithOutOTB = false;
            });
        }
        #endregion
        #region Commands
        public ICommand LogOutCommand {
            get
            {
                return new RelayCommand(()=> {

                        Application.Current.Properties.Clear();
                        App.Current.MainPage = new NavigationPage(new View_Login());
                });             
            }
                
        }
        #endregion
    }
}
