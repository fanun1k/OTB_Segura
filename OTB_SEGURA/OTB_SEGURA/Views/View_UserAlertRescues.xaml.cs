using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_UserAlertRescues : ContentPage
    {
        public View_UserAlertRescues()
        {
            InitializeComponent();
        }

        private void UserAlertsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}