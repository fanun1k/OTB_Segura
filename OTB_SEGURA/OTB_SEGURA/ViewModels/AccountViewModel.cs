using System;
using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using OTB_SEGURA.Views;

namespace OTB_SEGURA.ViewModels
{
    ///<summary>
    /// Nombre del desarrollador: Miguel Angel Terrazas Challapa
    /// Clase para realizar el update de usuario
    ///</summary>
    public class AccountViewModel : BaseViewModel
    {
        #region Attributes


        #endregion
        #region Properties

        #endregion
        #region Construct
        public AccountViewModel()
        {
            Title = "Editar Usuario"; //Titulo de View_Account
            DependencyService.Get<IMessage>().LongAlert(Application.Current.Properties["Ci"] as string);
        }

        #endregion
        #region Command      
        public ICommand UpdateCommand //Comando para llamar al metodo 
        {
            get
            {
                return new RelayCommand(UpdateMethod);
            }
        }
        #endregion
        #region Method
        /// <summary>
        /// Metodo update del perfil de usuario
        /// </summary>
        private async void UpdateMethod()
        {
            IsBusy = true;
            if (true)
            {

                //Se verifica si el password ingresado en los 2 campos es el mismo, esto como un metodo de seguridad
                //    if (password1 != password2)
                //    {
                //        await App.Current.MainPage.DisplayAlert("Error en la contraseña ", "Escriba la misma en ambos campos", "OK"); //Error de contraseña
                //    }
                //    else
                //    {
                //        var user = new UserModel
                //        {
                //            //Se agregan los atributos al constructor
                //            UserId = FireBaseHelper.staticUser.UserId, //userId de el usuario con sesion iniciada
                //            Name = name.ToUpper().Trim(),
                //            Ci = ci,
                //            Cell_phone = phone,
                //            State = 1,
                //            Photo = null,
                //            Password = password1,
                //            UserName = FireBaseHelper.staticUser.UserName,
                //            Email = FireBaseHelper.staticUser.Email,
                //            Type = FireBaseHelper.staticUser.Type
                //        };

                //        //await fireBaseHelper.UpdateUser(user);
                //        await Task.Delay(1000);
                //        DependencyService.Get<IMessage>().ShortAlert("Usuario Editado con éxito");
                //        await Shell.Current.GoToAsync("..");
                //    }
                //}

                //IsBusy = false;
            }
            /// <summary>
            /// Validacion de datos de entrada 
            /// </summary>
            /// <returns></returns>
            //private bool Validar()
            //{
            //    bool res;
            //    if (ci.ToString().Length > 5)
            //    {
            //        if (phone.ToString().Length > 6)
            //        {
            //            res = true;
            //        }
            //        else
            //        {
            //            res = false;
            //            DependencyService.Get<IMessage>().LongAlert("El número de celular debe tener más de 7 caracteres");

            //        }
            //    }
            //    else
            //    {
            //        DependencyService.Get<IMessage>().LongAlert("El número de carnet debe tener mas de 5 caracteres");
            //        res = false;
            //    }
            //    return res;
            //}
            #endregion
        }
    }
}
