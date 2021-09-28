﻿using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    ///<summary>
    /// Nombre del desarrollador: Miguel Angel Terrazas Challapa
    /// Clase para realizar el update de usuario
    ///</summary>
    public class ViewModel_EditProfile : BaseViewModel
    {
        #region Attributes
        private UserModel user = new UserModel();
        UserService restFull = new UserService();
        #endregion
        #region Properties
        public UserModel User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }
        #endregion
        #region Construct
        public ViewModel_EditProfile()
        {
            Title = "Editar Usuario"; //Titulo de View_Account
            User.Name = Application.Current.Properties["Name"] as string;
            User.Cell_phone = int.Parse(Application.Current.Properties["Phone"].ToString());
            User.User_ID = int.Parse(Application.Current.Properties["User_ID"].ToString());
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
            if (Validate())
            {
                try
                {
                    IsBusy = true;
                    user.State = 1;
                    ResponseHTTP<UserModel> resultHTTP = await restFull.UserUpdate(user);
                    if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                    {
                        Application.Current.Properties["Name"]= resultHTTP.Data[0].Name;
                        Application.Current.Properties["Phone"] = resultHTTP.Data[0].Cell_phone;
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
        }
        ///<summary>
        ///Validacion de datos de entrada 
        ///</summary>
        ///<returns></returns>
        private bool Validate()
        {
            bool res;
         
                if (user.Cell_phone.ToString().Length > 6)
                {
                    if (user.Name.Length > 4)
                    {
                        res = true;
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert("El nombre no puede ser tan reducido");
                        res = false;
                    }
                }               
                else
                {
                    res = false;
                    DependencyService.Get<IMessage>().LongAlert("El número de celular debe tener más de 7 caracteres");

                }
            return res;
        }
        #endregion
    }
}
