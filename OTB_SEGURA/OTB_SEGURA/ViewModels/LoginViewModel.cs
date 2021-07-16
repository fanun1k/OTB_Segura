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
        /// Variables de uso Global
        /// </summary>
        FireBaseHelper fireBaseHelper = new FireBaseHelper();
        UserModel userModel;

        /// <summary>
        /// Parte de declaracion de variables
        /// </summary>
        #region Variables
        private string userName = "";
        private string password = "";
        #endregion

        /// <summary>
        /// Getters y Setters
        /// </summary>
        #region GettersAndSetters
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }
        public bool RememberMe
        {
            get { return _rememberMe; }
            set { _rememberMe = value; this.OnPropertyChanged("ColorFiltered"); }
        }
        private bool _rememberMe = false;
        #endregion
        public Command LoginCommand { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Asigna el comando de OnLogiClicked a la variable LoginCommand
        /// </remarks>
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        /// <summary>
        /// Metodo para asignar un comando async en alguna variable command
        /// </summary>
        private async void OnLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//AboutPage");

        }

        /// <summary>
        /// Metodos ICommand
        /// </summary>
        #region Commands

        /// <summary>
        /// Devuelve el metodo LoginMethod
        /// </summary>
        public ICommand LoginValidateCommand
        {
            get
            {
                return new RelayCommand(LoginMethod);
            }
        }

        /// <summary>
        /// Devuelve el metodo LoginSuccess
        /// </summary>
        public ICommand Logged
        {
            get
            {
                return new RelayCommand(LoginSuccess);
            }
        }
        #endregion


        /// <summary>
        /// Metodos
        /// </summary>
        #region metodos

        /// <summary>
        /// Metodo Para realizar la accion de inicio de sesion
        /// </summary>
        /// <remarks>
        /// Esta clase realiza revision de existencia del usuario, 
        /// realiza la creacion de la sesion si es que fue requerida, 
        /// tiene conexion con la base de datos
        /// traslada al usuario a la ventana emergencia en caso de ser realizar inicio de sesion
        /// </remarks>
        private async void LoginMethod()
        {
            if (validar())
            {

                try
                {
                    userModel = await fireBaseHelper.GetPerson(userName.ToUpper(), password);
                    if (userModel.State != 0)
                    {
                        string tipo = "";
                        if (_rememberMe)
                        {
                            Application.Current.Properties["Sesion"] = 1;
                        }
                        Application.Current.Properties["Id"] = userModel.UserId;
                        Application.Current.Properties["Name"] = userModel.Name;
                        Application.Current.Properties["UserName"] = userModel.UserName;
                        Application.Current.Properties["Ci"] = userModel.Ci;
                        Application.Current.Properties["Password"] = userModel.Password;
                        Application.Current.Properties["Phone"] = userModel.Phone;
                        Application.Current.Properties["UserType"] = userModel.UserType;


                        if (userModel.UserType == 1)
                        {
                            tipo = "admin";
                        }
                        else tipo = "user";
                        MessagingCenter.Send<LoginViewModel>(this, tipo);
                        //DependencyService.Get<IMessage>().LongAlert(tipo);
                        DependencyService.Get<IMessage>().LongAlert("Bienvenido: " + userModel.Name);
                        await Shell.Current.GoToAsync("//AddActivity");

                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert("Su usuario se Encuentra bloqueado, comuniquese con soporte de su zona");
                    }

                }
                catch (Exception ex)
                {
                    DependencyService.Get<IMessage>().LongAlert("El usuario o la contraseña no son correctos");
                }
            }
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
            await Shell.Current.GoToAsync("//AddActivity");
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
                if (userName != "")
                {
                    if (password != "")
                    {
                        userName = userName.Replace(" ", string.Empty);
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
