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

namespace OTB_SEGURA.ViewModels
{
    public class RecoverPasswordViewModel:BaseViewModel
    {
        #region attributes
        private string correo;
        private FireBaseHelper fireBaseHelper = new FireBaseHelper();
        private ICommand recoveryCommand;
        private Func<string, string, string, Task> displayAlert;
        #endregion

        public RecoverPasswordViewModel(Func<string, string, string, Task> displayAlert)
        {
            this.displayAlert = displayAlert;
        }

        #region Properties
        public string Correo
        {
            get { return correo; }
            set { correo = value; OnPropertyChanged(); }
        }
        #endregion


        #region Commands
        public ICommand RecoveryCommand
        {
            get { return new RelayCommand(async()=> {
              if(await ValidateForm())
                {

                }
            
            }); }
        }
        #endregion

        #region Methods
        private async Task<bool> ValidateForm()
        {
            //Valida si el valor en el Entry txtTo se encuentra vacio o es igual a Null
            if (String.IsNullOrWhiteSpace(Correo))
            {
                await displayAlert("Advertencia", "Debe introducir un correo electrónico", "Ok");
                return false;
            }
            else
            {
                //Valida que el formato del correo sea valido
                bool isEmail = Regex.IsMatch(Correo, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isEmail)
                {
                    await displayAlert("Advertencia", "El formato del correo electrónico es incorrecto, revíselo e intente nuevamente.", "OK");
                    return false;
                }
            }          
            
            return true;
        }

        private async Task<bool> ValidateEmail()
        {
          UserModel user= await fireBaseHelper.ValidateEmail(Correo);
            //valida si existe un usuario en bdd con ese email
            if (user==null)
            {
                await displayAlert("Advertencia", "El correo que especificó no pertenece a ningun usuario con cuenta activa", "OK");
                return false;
            }
            return true;
        }

            #endregion



        }
}
