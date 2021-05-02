using OTB_SEGURA.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OTB_SEGURA
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
