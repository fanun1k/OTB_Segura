using System;
using System.Collections.Generic;
using System.Text;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    
    public class AddUSerViewModel:BaseViewModel
    {

        #region Attributes
        FireBaseHelper fireBaseHelper = new FireBaseHelper();
        private string name="";
        private string userName;
        private int ci;
        private int phone;
        private string password;
        private string rePassword;
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
            if (ci.ToString().Length > 7)
            {
                if (phone.ToString().Length > 6)
                {
                    if (password.Length>5)
                    {
                        if (password.Trim()==rePassword.Trim())
                        {
                            res = true;
                        }
                        else
                        {
                            res = false;
                            DependencyService.Get<IMessage>().LongAlert("las contraseñas deben ser iguales");
                        }
                    }
                    else
                    {
                        res = false;
                        DependencyService.Get<IMessage>().LongAlert("las contraseña debe tener mas de 5 caracteres");

                    }
                }
                else
                {
                    res = false;
                    DependencyService.Get<IMessage>().LongAlert("El número de celular debe tener más de 7 caracteres");

                }
            }
            else
            {
                DependencyService.Get<IMessage>().LongAlert("El número de carnet debe tener 8 caracteres");
                res = false;
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
        #endregion



    }
}
