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
            MessagingCenter.Subscribe<LoginViewModel>(this,"admin",(sender)=> {
                IsAdmin = true;
                DependencyService.Get<IMessage>().LongAlert("admin");

            });
            MessagingCenter.Subscribe<LoginViewModel>(this, "user", (sender) => {
                IsAdmin = false;
                DependencyService.Get<IMessage>().LongAlert("user");

            });
            DependencyService.Get<IMessage>().LongAlert("appshell");
        }
    }
}
