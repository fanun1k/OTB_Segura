using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Services;
using Xamarin.Forms;
using OTB_SEGURA.Models;
using System.Net.Mail;

namespace OTB_SEGURA.ViewModels
{
    public class RecoverPasswordViewModel:BaseViewModel
    {
        #region attributes
        private Func<string, string, string, Task> displayAlert;
        UserService userService = new UserService();
        private string  email;
        private string ci;
        public INavigation Navigation { get; set; }
        #endregion

        #region Properties
        public string Ci
        {
            get { return ci; }
            set { ci = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        #endregion

        #region Construct
        public RecoverPasswordViewModel(Func<string, string, string, Task> displayAlert,INavigation nav)
        {
            this.displayAlert = displayAlert;
            Navigation = nav;

        }
        #endregion

        #region Commands
        public ICommand RecoveryCommand
        {
            get { 
                return new Command(async(obj) => {
                    IsBusy = false;
                    ((Command)RecoveryCommand).ChangeCanExecute();
                    if (await ValidateForm())
                    {
                        if (await ValidateForm())
                        {
                        
                            try
                            {
                                ResponseHTTP<UserModel> responseHTTP = await userService.RecoveryPassword(Email.Trim(),Ci.Trim());
                                if (responseHTTP.Code==System.Net.HttpStatusCode.OK)
                                {
                                    DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                                    await Navigation.PopAsync();
                                }
                                else
                                {
                                    DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                                }
                            }
                            catch (Exception ex)
                            {
                                DependencyService.Get<IMessage>().LongAlert("Error " + ex.Message);
                            }
                        }
                    }
                    IsBusy = true;
                },canExecute: (obj) => { return IsBusy; }); 
            
            }
        }
        #endregion

        #region Methods
        private async Task<bool> ValidateForm()
        {
            //Valida si el valor en el Entry txtTo se encuentra vacio o es igual a Null
            if (String.IsNullOrWhiteSpace(Email.Trim()))
            {
                await displayAlert("Advertencia", "Debe introducir un correo electrónico", "Ok");
                return false;
            }
            else
            {
                //Valida que el formato del correo sea valido
                bool isEmail = Regex.IsMatch(Email.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isEmail)
                {
                    await displayAlert("Advertencia", "El formato del correo electrónico es incorrecto, revíselo e intente nuevamente.", "OK");
                    return false;
                }
            }          
            
            return true;
        }
            #endregion

    }
}
