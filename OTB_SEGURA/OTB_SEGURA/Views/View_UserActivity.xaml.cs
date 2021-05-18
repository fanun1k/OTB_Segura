using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OTB_SEGURA.Models;
using OTB_SEGURA.ViewModels;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_UserActivity : ContentPage
    {
        public View_UserActivity()
        {
            InitializeComponent();
        }

        private async void ListActivityUser_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            ListActivityUser.EndRefresh();
        }
    }
}