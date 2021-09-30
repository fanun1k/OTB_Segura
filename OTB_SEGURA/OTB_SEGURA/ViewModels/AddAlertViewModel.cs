using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    class AddAlertViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand AddAlertCommand => new Command(AddAlert);
        public ICommand RemoveAlertCommand => new Command(RemoveAlert);
        public ObservableCollection<string> Alerts { get; set; }

        public string Alertname { get; set; }
        public string SelectedAlert { get; set; }
     

        public AddAlertViewModel(INavigation nav)
        {
            Navigation = nav;
            Alerts = new ObservableCollection<string>();

            Alerts.Add("Robo");
            Alerts.Add("Incendio");
            Alerts.Add("Accidente");
            Alerts.Add("Inundacion");
            
        }

        public void AddAlert()
        {
            Alerts.Add(Alertname);
        }
        public void RemoveAlert()
        {
            Alerts.Remove(SelectedAlert);
        }
        
        public ICommand InfoCommand 
        {
            get
            {
                return new RelayCommand(async()=> {
                    await Navigation.PushAsync(new View_HelpAlert());
                });
            }
        }
    }
}
