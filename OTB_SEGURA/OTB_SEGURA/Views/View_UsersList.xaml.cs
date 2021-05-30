using OTB_SEGURA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OTB_SEGURA.ViewModels;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_UsersList : ContentPage
    {
        public View_UsersList()
        {
            InitializeComponent();
            BindingContext = new UsersListViewModel(Navigation);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new View_AddUser());
        }

        private async void ListUser_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            ListUser.EndRefresh();
        }
    }
}