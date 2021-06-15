using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    public  class AppShellViewModel:BaseViewModel
    {
        private bool isAdmin;

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; OnPropertyChanged(); }
        }

        public AppShellViewModel()
        {
            if (Application.Current.Properties.ContainsKey("Sesion"))
            {
                if (int.Parse(Application.Current.Properties["UserType"].ToString()) == 1)
                {
                    IsAdmin = true;
                }
            }         
            MessagingCenter.Subscribe<LoginViewModel>(this,"admin",(sender)=> {
                IsAdmin = true;
            });
            MessagingCenter.Subscribe<LoginViewModel>(this, "user", (sender) => {
                IsAdmin = false;
            });
        }
    }
}
