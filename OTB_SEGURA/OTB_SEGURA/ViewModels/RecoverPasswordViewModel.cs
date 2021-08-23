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
        private string correo;
        private FireBaseHelper fireBaseHelper = new FireBaseHelper();
        private Func<string, string, string, Task> displayAlert;
        private UserModel user;  
        private string newPass="";
        #endregion

        #region Properties
        public string Correo
        {
            get { return correo; }
            set { correo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Construct
        public RecoverPasswordViewModel(Func<string, string, string, Task> displayAlert)
        {
            this.displayAlert = displayAlert;
        }
        #endregion

        #region Commands
        public ICommand RecoveryCommand
        {
            get { return new RelayCommand(async()=> {
              if(await ValidateForm())
                {
                    if (await ValidateEmail())
                    {
                        
                        try
                        {
                            //generando nueva contraseña
                            Random rand = new Random();
                            int aux = rand.Next(111, 999);
                            newPass = "";
                            for (int i = 0; i < 4; i++)
                            {
                                int numero = rand.Next(26);
                                char letra = (char)(((int)'A') + numero);
                                newPass += letra;
                            }
                            newPass += aux;

                            user.Password = newPass;
                            await fireBaseHelper.UpdateUser(user);
                            //enviando la nueva contraseña al correo
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                            mail.From = new MailAddress("emergencyproject2@gmail.com");
                            mail.To.Add(Correo);
                            mail.Subject = "OTB SEGURA - Restablecer Contraseña";
                            mail.Body = $"Su contraseña en la aplicacion OTB SEGURA fué restablecida, su nueva contraseña es: {newPass}. No olvide cambiar su contraseña despues de iniciar sesión";

                            SmtpServer.Port = 587;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("emergencyproject2@gmail.com", "proyecto2021");
                            SmtpServer.EnableSsl = true;

                            SmtpServer.Send(mail);
                            DependencyService.Get<IMessage>().LongAlert("Se le envio su nueva contraseña al correo  "+correo);
                        }
                        catch (Exception ex)
                        {
                            DependencyService.Get<IMessage>().LongAlert("Error " + ex.Message);
                        }
                    }
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
            user = null;
            user = await fireBaseHelper.ValidateEmail(Correo);
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
