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
        FireBaseHelper fireBaseHelper = new FireBaseHelper();
        #region Attributes

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


        public string UserName
        {
            get { return userName; }
            set { userName = value;}
        }


        public string Name
        {
            get { return name; }
            set { name = value;
                CreateUserName();
            }
        }

        #endregion
        public AddUSerViewModel()
        {
            Title = "Agregar Nuevo Usuario";
        }
        #region Command
        public ICommand InsertCommand
        {
            get
            {
                return new RelayCommand(InsertMethod);
            }
        }
        #endregion
        
        #region Metodh
        private async void InsertMethod()
        {
            var user = new UserModel
            {
                Name = name,
                UserName = userName,
                Password = password,
                Ci = ci,
                Phone = phone,
                State=1,
                Photo=null

            };

            await fireBaseHelper.AddUser(user);
            await Task.Delay(1000);
            DependencyService.Get<IMessage>().LongAlert("Usuario Agregado con éxito");
        }

        public void CreateUserName()
        {
            try
            {
                if (name.Length>0)
                {
                    string[] iniciales = name.Split(' ');
                    string userNameComplete = "";
                    foreach (string inicial in iniciales)
                    {
                        userNameComplete += inicial.Substring(0, 1).ToUpper();
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
