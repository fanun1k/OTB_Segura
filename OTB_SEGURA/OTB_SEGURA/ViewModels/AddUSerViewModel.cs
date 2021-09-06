using System;
using System.Collections.Generic;
using System.Text;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;
using System.Text.RegularExpressions;

namespace OTB_SEGURA.ViewModels
{
    /// <summary>
    /// clase AddUSerViewModel que nos sirbe de logica para la vista View_AdUser
    /// </summary>
    public class AddUSerViewModel:BaseViewModel
    {


        #region Attributes
        private UserModel user = new UserModel();
        UserService restFull = new UserService();
        private string rePassword = "";

        #endregion

        #region Properties

        public UserModel User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }

        public string RePassword
        {
            get { return rePassword; }
            set { rePassword = value; }
        }
        #endregion

        #region Construct
        /// <summary>
        /// Constructor que asigna el titulo a la vista View_AddUser
        /// </summary>
        public AddUSerViewModel()
        {
            Title = "Agregar Nuevo Usuario";
        }
        #endregion

        #region Command
        /// <summary>
        /// comando que se ejecuta cuando se hace click al boton de insertar en la vista
        /// </summary>
        public ICommand InsertCommand
        {
            get
            {
                return new RelayCommand(InsertMethod);
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Metodo que nos ayuda a Hacer la insercion de un usuario a la bdd
        /// </summary>
        private async void InsertMethod()
        {         
            try
            {
                IsBusy = true;
                user.State = 1;
                ResponseHTTP<UserModel> resultHTTP = await restFull.UserInsert(user);
                if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                {
                    DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }                            
        }

        /// <summary>
        /// Metodo que valida los campos del formulario de registro de usuario de la vista 
        /// </summary>
        /// <returns></returns>
        private bool Validar()
        {
            bool res;

            if (!Regex.Match(user.Name, "^[ñA-Za-z _]*[ñA-Za-z][ñA-Za-z _]*$").Success)
            {
                res = false;
                DependencyService.Get<IMessage>().LongAlert("Formato del nombre incorrecto");
            }
            else if (!Regex.Match(user.Ci.ToString(), "^[0-9]{7}$").Success)
            {
                res = false;
                DependencyService.Get<IMessage>().LongAlert("Formato de C.I incorrecto");
            }
            else if (!Regex.Match(user.Cell_phone.ToString(), "^[0-9]{8}$").Success)
            {
                res = false;
                DependencyService.Get<IMessage>().LongAlert("Número de télefono incorrecto");
            }
            else if (!Regex.Match(user.Email.ToString(), "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*").Success)
            {
                res = false;
                DependencyService.Get<IMessage>().LongAlert("Correo invalido");
            }
            else
            {
                if (!user.Password.Equals(""))
                {
                    if (!rePassword.Equals(""))
                    {
                        if (user.Password.Length > 5)
                        {
                            if (user.Password.Trim() == rePassword.Trim())
                            {
                                res = true;
                            }
                            else
                            {
                                res = false;
                                DependencyService.Get<IMessage>().LongAlert("Las contraseñas no coinciden");
                            }
                        }
                        else
                        {
                            res = false;
                            DependencyService.Get<IMessage>().LongAlert("La contraseña debe tener más de 5 caracteres");
                        }
                    }
                    else
                    {
                        res = false;
                        DependencyService.Get<IMessage>().LongAlert("Por favor, confirme la contraseña");
                    }
                }
                else
                {
                    res = false;
                    DependencyService.Get<IMessage>().LongAlert("Por favor, introduzca una contraseña");
                }
            }
            return res;
        }
      
        #endregion


    }
}
