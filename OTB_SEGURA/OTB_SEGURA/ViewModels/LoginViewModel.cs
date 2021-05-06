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
        private string userName="";
        private string password="";
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
        #endregion
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
                await Shell.Current.GoToAsync($"//{nameof(View_About)}");
            
        }
        #region Commands
        public ICommand LoginValidateCommand
        {
            get
            {
                return new RelayCommand(LoginMethod);
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
                    DependencyService.Get<IMessage>().LongAlert("Bienvenido: "+userModel.Name);
                }
                catch(Exception ex)
                {
                    DependencyService.Get<IMessage>().LongAlert("El usuario o la contraseña no son correctos");
                }
            }
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
            catch(Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
            return res;
        }
        #endregion
    }
}
