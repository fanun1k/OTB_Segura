using OTB_SEGURA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_EmergencyNumbers : ContentPage
    {
        EmergencyNumbersViewModel emergencyNumbersList = new EmergencyNumbersViewModel();
        public View_EmergencyNumbers()
        {
            InitializeComponent();
        }
        private void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                ShowExitDialog(e);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error al marcar ", "intente nuevamente", "OK", "cancel");
            }
        }

        private async void ShowExitDialog(SelectedItemChangedEventArgs e)
        {
            var number = emergencyNumbersList.emergencyNumbersList[e.SelectedItemIndex].Number;
            var answer = await DisplayAlert("Llamar a " + emergencyNumbersList.emergencyNumbersList[e.SelectedItemIndex].Name, "¿Desea realizar la llamada?", "Aceptar", "Cancelar");

            if (answer)
            {
                //await Navigation.PushAsync(new View_EmergencyNumberUserList());
                PhoneDialer.Open(number.ToString());
            }
        }
    }
}