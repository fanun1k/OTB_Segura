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
    class AccountViewModel : BaseViewModel
    {
        #region Attributes
        FireBaseHelper fireBaseHelper = new FireBaseHelper();
        private string name = "";
        private int ci;
        private int phone;
        private string password1;
        private string password2;
        #endregion
        #region Properties
        public int Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public int Ci
        {
            get { return ci; }
            set
            {
                ci = value;
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        public string Password1
        {
            get { return password1; }
            set
            {
                password1 = value;
            }
        }
        public string Password2
        {
            get { return password2; }
            set
            {
                password2 = value;
            }
        }
        #endregion
        #region Construct
        public AccountViewModel()
        {
            Title = "Editar Usuario";
        }
        #endregion
        #region Command      
        public ICommand UpdateCommand
        {
            get
            {
                return new RelayCommand(UpdateMethod);
            }
        }
        #endregion
        #region Method
        
        private async void UpdateMethod()
        {
            IsBusy = true;
            if (Validar())
            {
                if (password1 != password2)
                {
                    //agregar alerta
                    View_Account vi = new View_Account();
                    vi.DisplayAlert("Error en la contraseña ", "Escriba la misma en ambos campos", "OK");
                }
                else
                { 
                var user = new UserModel
                {
                    UserId = FireBaseHelper.staticUser.UserId,
                    Name = name.ToUpper().Trim(),
                    Ci = ci,
                    Phone = phone,
                    State = 1,
                    Photo = null,
                    Password = password1,
                    UserName = FireBaseHelper.staticUser.UserName

                };
                
                await fireBaseHelper.UpdateUser(user);
                await Task.Delay(1000);
                DependencyService.Get<IMessage>().ShortAlert("Usuario Editado con éxito");
                await Shell.Current.GoToAsync("..");
                }
            }

            IsBusy = false;
        }

        private bool Validar()
        {
            bool res;
            if (ci.ToString().Length > 5)
            {
                if (phone.ToString().Length > 6)
                {
                    res = true;
                }
                else
                {
                    res = false;
                    DependencyService.Get<IMessage>().LongAlert("El número de celular debe tener más de 7 caracteres");

                }
            }
            else
            {
                DependencyService.Get<IMessage>().LongAlert("El número de carnet debe tener mas de 5 caracteres");
                res = false;
            }
            return res;
        }
        #endregion
    }
}