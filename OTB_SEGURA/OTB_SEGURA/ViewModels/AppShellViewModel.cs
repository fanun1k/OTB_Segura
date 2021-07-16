using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    /// <summary>
    /// Clase que nos sirbe como logica para la interfaz del menu hamburgueza AppShell
    /// </summary>
    public  class AppShellViewModel:BaseViewModel
    {
        #region Attributes
        private bool isAdmin;
        #endregion
        #region Properties
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; OnPropertyChanged(); }
        }
        #endregion
        #region Construct
        /// <summary>
        /// Constructor que nos sirbe para determinar si el usuario que inicio sesion es administrador o usuario
        /// Esto oculta algunas caracteristicas de la vista
        /// </summary>
        public AppShellViewModel()
        {
            if (Application.Current.Properties.ContainsKey("Sesion"))
            {
                if (int.Parse(Application.Current.Properties["UserType"].ToString()) == 1)
                {
                    IsAdmin = true;
                }
            }         
            MessagingCenter.Subscribe<LoginViewModel>(this,"admin",(sender)=> {
                IsAdmin = true;
            });
            MessagingCenter.Subscribe<LoginViewModel>(this, "user", (sender) => {
                IsAdmin = false;
            });
        }
        #endregion
    }
}
