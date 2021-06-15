using OTB_SEGURA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OTB_SEGURA.Models;
namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_EmergencyNumberUserList : ContentPage
    {
        AccountViewModel account = new AccountViewModel();
        public View_EmergencyNumberUserList()
        {
            InitializeComponent();
        }

        private void ListUser_Refreshing(object sender, EventArgs e)
        {

        }

        private void ListUser_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                ShowExitDialog(e);
            }
            catch (Exception)
            {

                DisplayAlert("Error al ingresar al perfil ", "intente nuevamente", "OK", "Cancelar");
            }
        }

        private async void ShowExitDialog(SelectedItemChangedEventArgs e)
        {
            UserModel user = e.SelectedItem as UserModel;
            //DependencyService.Get<IMessage>().ShortAlert(user.Name + user.Phone);
            //Agregar redireccionamiento a perfil
            await Navigation.PushAsync(new View_UserProfile(user.Name,user.Phone,user.UserId));
        }
    }
}