﻿using System;
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
        public AddUSerViewModel()
        {
            Title = "Agregar Nuevo Usuario";
        }
        #endregion

        #region Command
        public ICommand InsertCommand
        {
            get
            {
                return new RelayCommand(InsertMethod);
            }
        }
        public ICommand OpenMapsCommand
        {
            get
            {
                return new RelayCommand(OpeenMaps);
            }
        }
        #endregion

        #region Method
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
                    Photo = null

                };
                await fireBaseHelper.AddUser(user);
                await Task.Delay(1000);
                DependencyService.Get<IMessage>().ShortAlert("Usuario Agregado con éxito");
                await Shell.Current.GoToAsync("..");
            }
            
            IsBusy = false;
        }

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

        private async void OpeenMaps()
        {
           Position position = null;
			try
			{
              
					var locator = CrossGeolocator.Current;
					locator.DesiredAccuracy = 100;

					position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
                    await Map.OpenAsync(position.Latitude, position.Longitude, new MapLaunchOptions
                    {
                        Name = "Ubicación",
                        NavigationMode = NavigationMode.None
                    });              
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
					{
                    //not available or enabled                    
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
