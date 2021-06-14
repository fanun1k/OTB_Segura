using OTB_SEGURA.ViewModels;
using OTB_SEGURA.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OTB_SEGURA
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Application.Current.Properties.Clear();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
