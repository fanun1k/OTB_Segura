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
    public partial class View_Login : ContentPage
    {
        public View_Login()
        {
            string name = null;
            InitializeComponent();
            LoginViewModel log = new LoginViewModel();
            this.BindingContext = new LoginViewModel();
            if (Application.Current.Properties.ContainsKey("Sesion"))
            {
                name = Application.Current.Properties["Name"] as string;
                DependencyService.Get<IMessage>().LongAlert("Bienvenido: " + name);
                log.Logged.Execute(button1);
            }
        }

    }
}