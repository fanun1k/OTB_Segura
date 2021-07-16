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
        FireBaseHelper fireBaseHelper = new FireBaseHelper();
        private string name="";
        private string userName="";
        private int ci=0;
        private int phone=0;
        private string password="";
        private string rePassword="";
        #endregion

        #region Properties
        public string RePassword
        {
            get { return rePassword; }
            set { rePassword = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }


        public int Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public int Ci
        {
            get { return ci; }
            set { ci = value;
               CreateUserName();
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value;
                CreateUserName();
            }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value;
                OnPropertyChanged();
            }
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
            IsBusy = true;
            if (Validar())
            {
                var user = new UserModel
                {
                    Name = name.ToUpper().Trim(),
                    UserName = userName,
                    Password = password.Trim(),
                    Ci = ci,
                    Phone = phone,
                    State = 1,
                    Photo = null,
                    UserType=0
                    
                };
                await fireBaseHelper.AddUser(user);
                await Task.Delay(1000);
                DependencyService.Get<IMessage>().ShortAlert("Usuario Agregado con éxito");
                await Shell.Current.GoToAsync("..");
            }
            
            IsBusy = false;
        }

        /// <summary>
        /// Metodo que valida los campos del formulario de registro de usuario de la vista 
        /// </summary>
        /// <returns></returns>
        private bool Validar()
        {
            bool res;

            if (!Regex.Match(name, "^[ñA-Za-z _]*[ñA-Za-z][ñA-Za-z _]*$").Success)
            {
                res = false;
                DependencyService.Get<IMessage>().LongAlert("Formato del nombre incorrecto");
            }
            else if (!Regex.Match(ci.ToString(), "^[0-9]{7}$").Success)
            {
                res = false;
                DependencyService.Get<IMessage>().LongAlert("Formato de C.I incorrecto");
            }
            else if (!Regex.Match(phone.ToString(), "^[0-9]{8}$").Success)
            {
                res = false;
                DependencyService.Get<IMessage>().LongAlert("Número de télefono incorrecto");
            }
            else
            {
                 if(!password.Equals(""))
                 {
                     if (!rePassword.Equals(""))
                     {
                         if (password.Length > 5)
                         {
                             if (password.Trim() == rePassword.Trim())
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

        /// <summary>
        /// Metodo que Crea un Nombre de usuario con las iniciales del nombre y el carnet
        /// </summary>
        public void CreateUserName()
        {
            try
            {
                if (name.Length > 0)
                {
                    string[] iniciales = name.Split(' ');
                    string userNameComplete = "";
                    foreach (string inicial in iniciales)
                    {
                        if (inicial.Length > 0)
                        {
                            userNameComplete += inicial.Substring(0, 1).ToUpper();
                        }
                    }
                    userNameComplete += Ci.ToString();                 
                    UserName = userNameComplete;
                  
                }
                         
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
            
        }
        #endregion



    }
}
