using GalaSoft.MvvmLight.Command;
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
    public class AccountViewModel : BaseViewModel
    {
        #region Attributes
        private UserModel user = new UserModel();
        UserService restFull = new UserService();
        private string oldPassword;

        #endregion
        #region Properties
        public string OldPassword
        {
            get { return oldPassword; }
            set { oldPassword = value; OnPropertyChanged(); }
        }

        public UserModel User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }
        #endregion
        #region Construct
        public AccountViewModel()
        {
            Title = "Editar Usuario"; //Titulo de View_Account
            User.Name = Application.Current.Properties["Name"] as string;
            User.Cell_phone = int.Parse(Application.Current.Properties["Phone"].ToString());
        }

        #endregion
        #region Command      
        public ICommand UpdateCommand //Comando para llamar al metodo y Metodo update del perfil de usuario
        {
            get
            {
                return new Command(execute: async (obj) => {
                    try
                    {
                        IsBusy = false;
                        ((Command)UpdateCommand).ChangeCanExecute();
                        user.State = 1;
                        if (Validate())
                        {
                            ResponseHTTP<UserModel> resultHTTP = await restFull.UserUpdate(user);
                            if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                            {
                                Application.Current.Properties["Name"] = resultHTTP.Data[0].Name;
                                Application.Current.Properties["Password"] = resultHTTP.Data[0].Password;
                                Application.Current.Properties["Phone"] = resultHTTP.Data[0].Cell_phone;
                                await Shell.Current.GoToAsync("..");
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                            }
                        }
                            
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                    IsBusy = true;
                }, canExecute: (obj) => { return IsBusy; });
            }
        }
        #endregion
        #region Method
        ///<summary>
        ///Validacion de datos de entrada 
        ///</summary>
        ///<returns></returns>
        private bool Validate()
        {
            bool res;
            if (oldPassword == Application.Current.Properties["Password"].ToString())
            {
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
            }
            else
            {
                DependencyService.Get<IMessage>().LongAlert("La contraseña antigua es incorrecta");
                res = false;
            }
            return res;
        }
        #endregion
    }
}
