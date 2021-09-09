using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTB_SEGURA.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_Account : ContentPage
    {
        public View_Account()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
           txtName.Text = Application.Current.Properties["Name"] as string;
           txtCi.Text = Application.Current.Properties["Ci"] as string;
           txtPhone.Text = Application.Current.Properties["Phone"] as string;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}