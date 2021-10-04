using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using OTB_SEGURA.Views;
using OTB_SEGURA.Services;
using OTB_SEGURA.Models;

using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace OTB_SEGURA.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        /// <summary>
        /// Parte de declaracion de variables
        /// </summary>
        #region Attributes
        private UserModel user=new UserModel();
        UserService restFull = new UserService();
        INavigation navigation;
        #endregion
        /// <summary>
        /// Getters y Setters
        /// </summary>
        #region Properties
        public UserModel User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }
        public bool RememberMe
        {
            get { return _rememberMe; }
            set { _rememberMe = value; this.OnPropertyChanged("ColorFiltered"); }
        }
        private bool _rememberMe = true;
        #endregion
        public Command LoginCommand { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// </remarks>
        public LoginViewModel(INavigation nav)
        {
            navigation = nav;
            if (Application.Current.Properties.ContainsKey("Sesion"))
            {
                 Shell.Current.GoToAsync("//MyProfile");
            }
        }



        /// <summary>
        /// Metodos ICommand
        /// </summary>
        #region Commands

        /// <summary>
        /// Devuelve el metodo LoginMethod
        /// </summary>
        /// 
        /// <summary>
        /// Metodo Para realizar la accion de inicio de sesion
        /// </summary>
        /// <remarks>
        /// Esta clase realiza revision de existencia del usuario, 
        /// realiza la creacion de la sesion si es que fue requerida, 
        /// tiene conexion con la base de datos
        /// traslada al usuario a la ventana emergencia en caso de ser realizar inicio de sesion
        /// </remarks>
        public ICommand LoginValidateCommand
        {
            get
            {
                return new Command(execute: async (obj) => {
                    IsBusy = false;
                    ((Command)LoginValidateCommand).ChangeCanExecute();
                    var re = await restFull.UsersByOtb(5);
                    if (validar())
                    {

                        try
                        {
                            ResponseHTTP<UserModel> resultHTTP = await restFull.Login(User.Email, user.Password);
                            if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                            {
                                if (resultHTTP.Data[0].State != 0)
                                {
                                    string tipo = "";

                                    if (_rememberMe)
                                    {
                                        Application.Current.Properties["Sesion"] = 1;
                                    }
                                    Application.Current.Properties["User_ID"] = resultHTTP.Data[0].User_ID;
                                    Application.Current.Properties["Id"] = resultHTTP.Data[0].UserId;
                                    Application.Current.Properties["Name"] = resultHTTP.Data[0].Name;
                                    Application.Current.Properties["Email"] = resultHTTP.Data[0].Email;
                                    Application.Current.Properties["Ci"] = resultHTTP.Data[0].Ci;
                                    Application.Current.Properties["Password"] = resultHTTP.Data[0].Password;
                                    Application.Current.Properties["Phone"] = resultHTTP.Data[0].Cell_phone;
                                    Application.Current.Properties["UserType"] = resultHTTP.Data[0].Type;
                                    Application.Current.Properties["Otb_ID"] = resultHTTP.Data[0].Otb_ID;
                                    Application.Current.Properties["Token"] = resultHTTP.Data[0].Token;

                                    if (resultHTTP.Data[0].Type >= 1)
                                    {
                                        tipo = "admin";
                                    }
                                    else tipo = "user";
                                    if (resultHTTP.Data[0].Type == 0 && resultHTTP.Data[0].Otb_ID == null)
                                        tipo = "userWithOutOTB";
                                    MessagingCenter.Send<LoginViewModel>(this, tipo);
                                    //DependencyService.Get<IMessage>().LongAlert(tipo);
                                    //DependencyService.Get<IMessage>().LongAlert("Bienvenido: " + resultHTTP.Data[0].Name);
                                    DependencyService.Get<IMessage>().LongAlert(Application.Current.Properties["Name"] as string);
                                    await Shell.Current.GoToAsync("//MyProfile");

                                }
                                else
                                {
                                    DependencyService.Get<IMessage>().LongAlert("Su usuario se Encuentra bloqueado, comuniquese con soporte de su zona");
                                }
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                            }
                        }
                        catch (Exception ex)
                        {
                            DependencyService.Get<IMessage>().LongAlert(ex.Message);
                        }
                    }
                    IsBusy = true;
                }, canExecute: (obj) => { return IsBusy; });
            }
        }

        public ICommand RecoveryPassCommand
        {
            get
            {
                return new RelayCommand(async()=>{
                    await navigation.PushAsync(new View_RecoverPassword());
                });
            }
        }

        public ICommand CreateAccountCommand
        {
            get
            {
                return new RelayCommand(async () => {
                    await navigation.PushAsync(new View_AddUser());
                });
            }
        }
        #endregion

        /// <summary>
        /// Metodos
        /// </summary>
        #region metodos

        /// <summary>
        /// Metodo para asignar un comando async en alguna variable command
        /// </summary>
        private async void OnLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//AboutPage");

        }

        /// <summary>
        /// Metodo Para realizar la accion de inicio de sesion
        /// </summary>
        /// <remarks>
        /// Este metodo, se lleva a cabo solo si la sesion fue creada, lo transportara
        /// directo a pagina de emergencia
        /// </remarks>
        public async void LoginSuccess()
        {
            await Shell.Current.GoToAsync("//MyProfile");
        }

        /// <summary>
        /// Metodo Para validar entries ingresados por el usuario
        /// </summary>
        /// <remarks>
        /// Este metodo, revisa la validez de los entries ingresados por el usuario
        /// revisa si estan vacios, y tambien revisa caracteres como espacios
        /// da una alerta de lo que se consiguio
        /// </remarks>
        public bool validar()
        {
            bool res = false;
            try
            {
                if (user.Email != "")
                {
                    if (user.Password != "")
                    {
                        user.Email = user.Email.Replace(" ", string.Empty);
                        res = true;
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert("Llene el campo de Contraseña");
                    }
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert("Todos los campos tienen que llenados");
                }

            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
            return res;
        }
        #endregion
    }
}
