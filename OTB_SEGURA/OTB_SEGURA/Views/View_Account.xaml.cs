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
            txtName.Text = FireBaseHelper.staticUser.Name.ToString();
            txtCi.Text = FireBaseHelper.staticUser.Ci.ToString();
            txtPhone.Text = FireBaseHelper.staticUser.Phone.ToString();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}