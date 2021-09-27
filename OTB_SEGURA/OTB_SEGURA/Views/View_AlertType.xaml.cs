using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTB_SEGURA.Models;
using OTB_SEGURA.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_AlarmType : ContentPage
    {
        
        public View_AlarmType(string title)
        {
            InitializeComponent();
            this.BindingContext = new ViewModel_Alert(title);

            
        }
        private async void ListActivityUser_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            ListActivityUser.EndRefresh();
        }
    }
}