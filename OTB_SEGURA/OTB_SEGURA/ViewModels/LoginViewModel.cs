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

        FireBaseHelper fireBaseHelper = new FireBaseHelper();
        UserModel userModel;

        #region Variables
        private string userName = "";
        private string password = "";
        #endregion

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

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//AboutPage");

        }
        #region Commands
        public ICommand LoginValidateCommand
        {
            get
            {
                return new RelayCommand(LoginMethod);
            }
        }
        public ICommand Logged
        {
            get
            {
                return new RelayCommand(LoginSuccess);
            }
        }
        #endregion

        #region metodos
        private async void LoginMethod()
        {
            if (validar())
            {

                try
                {
                    userModel = await fireBaseHelper.GetPerson(userName, password);
                    if (userModel.State != 0)
                    {

                        if (_rememberMe)
                        {
                            Application.Current.Properties["Id"] = userModel.UserId;
                            Application.Current.Properties["Name"] = userModel.Name;
                            Application.Current.Properties["UserName"] = userModel.UserName;
                            Application.Current.Properties["Ci"] = userModel.Ci;
                            Application.Current.Properties["Password"] = userModel.Password;
                            Application.Current.Properties["Phone"] = userModel.Phone;
                        }
                        DependencyService.Get<IMessage>().LongAlert("Bienvenido: " + userModel.Name);
                        await Shell.Current.GoToAsync("//AboutPage");

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
        public async void LoginSuccess()
        {
            await Shell.Current.GoToAsync("//AboutPage");
        }
        public bool validar()
        {
            bool res = false;
            try
            {
                if (userName != "")
                {
                    if (password != "")
                    {
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
