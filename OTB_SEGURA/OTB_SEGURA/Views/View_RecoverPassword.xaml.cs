using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTB_SEGURA.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_RecoverPassword : ContentPage
    {
        public View_RecoverPassword()
        {
            InitializeComponent();
            BindingContext = new RecoverPasswordViewModel(DisplayAlert,Navigation); 
        }
    }
}