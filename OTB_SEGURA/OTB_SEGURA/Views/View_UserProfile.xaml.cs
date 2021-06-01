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
        public View_UserProfile()
        {
            InitializeComponent();
            BindingContext = new UserProfileViewModel(Navigation);
        }
        public View_UserProfile(UserModel user)
        {
            InitializeComponent();            
            BindingContext = new UserProfileViewModel(user);
        }
        public View_UserProfile(string name,int phone)
        {
            InitializeComponent();
            BindingContext = new UserProfileViewModel(name,phone);
        }
    }
}