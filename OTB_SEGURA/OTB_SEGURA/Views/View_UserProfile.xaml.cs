using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTB_SEGURA.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OTB_SEGURA.Models;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_UserProfile : ContentPage
    {
        int aux = 0;
        public View_UserProfile()
        {
            InitializeComponent();
            BindingContext = new UserProfileViewModel(Navigation);
            aux = 1;
        }
        public View_UserProfile(UserModel user)
        {
            InitializeComponent();            
            BindingContext = new UserProfileViewModel(user);
            aux = 0;
        }
        public View_UserProfile(string name,int phone,Guid id)
        {
            InitializeComponent();
            BindingContext = new UserProfileViewModel(name,phone,id);
            aux = 0;
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (aux==1)
            {
                BindingContext = new UserProfileViewModel(Navigation);
            }
        }
    }
}