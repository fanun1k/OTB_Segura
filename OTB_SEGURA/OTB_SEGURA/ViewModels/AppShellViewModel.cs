using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    /// <summary>
    /// Clase que nos sirbe como logica para la interfaz del menu hamburgueza AppShell
    /// </summary>
    public  class AppShellViewModel:BaseViewModel
    {
        #region Attributes
        AlertService alertService = new AlertService();
        DataTemplate data;
         
        private bool isAdmin;
        //private bool isxolito;

        private UserService userService = new UserService();

        #endregion
        #region Properties
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; OnPropertyChanged(); }
        }
        private View_AlarmContainer alarmContainer;

        public View_AlarmContainer AlarmContainer
        {
            get { return alarmContainer; }
            set { alarmContainer= value; }
        }
        
        public DataTemplate Data
        {
            get { return data; }
            set { data = value; OnPropertyChanged(); }
        }

        /*public bool IsXolito
        {
            get { return isxolito; }
            set { isxolito = value; OnPropertyChanged(); }
        }*/
        #endregion
        #region Commands
        /*
        public ICommand ApperingCommand
        { 
            get 
            {
                return new RelayCommand(async() => {
                    try
                    {
                        int id = int.Parse(Application.Current.Properties["User_ID"].ToString());
                        ResponseHTTP<UserModel> responseHTTP = await userService.GetUser(id);
                        if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                        {
                            if (responseHTTP.Data[0].Type == 0)
                            {
                                IsAdmin = false;
                            }
                            if (responseHTTP.Data[0].State == 0)
                            {
                                await Shell.Current.GoToAsync("//LoginPage");
                            }
                            if (responseHTTP.Data[0].Otb_ID == null)
                            {
                                IsXolito = false;
                                IsAdmin = false;
                            }
                            else
                            {
                                IsXolito = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
            } 
        }
        */
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
            Data = new DataTemplate(() => {
                List<String> listaAlertas = new List<string>();
                listaAlertas.Add("General");
                listaAlertas.Add("Robos");
     
                //    ResponseHTTP<AlertModel> response = await alertService.listarAlertas(int.Parse(Application.Current.Properties["Otb_ID"].ToString()));
                //    if (response.Code == System.Net.HttpStatusCode.OK)
                //    {
                //        listaAlertas = response.Data;
                //    }
                //    else
                //    {
                //        DependencyService.Get<IMessage>().LongAlert(response.Msj);
                //    }
                //Data = new DataTemplate(() => { return new View_AlarmContainer(); });
                return new View_AlarmContainer(listaAlertas);
            });

        }
        #endregion
    }
}
