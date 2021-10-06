using OTB_SEGURA.Services;
using OTB_SEGURA.ViewModels;
using OTB_SEGURA.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OTB_SEGURA
{
    public partial class App : Application
    {
        static SQLiteHelper db;
        private AppShell appShell;
        private View_Login login;
        public App()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("Sesion"))
            {
                appShell = null;
                appShell = new AppShell();
                MainPage = appShell;
            }
            else
            {
                login = null;
                login = new View_Login();
                MainPage = login;
            }
        }
        public static SQLiteHelper SQLiteDB
        {
            get
            {
                if (db==null)
                {
                    db = new SQLiteHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"Otb.db3"));
                }
                return db;
            }
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
