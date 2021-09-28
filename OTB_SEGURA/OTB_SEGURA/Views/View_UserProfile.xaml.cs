using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTB_SEGURA.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OTB_SEGURA.Models;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_UserProfile : ContentPage
    {
#pragma warning disable IDE0044 // Agregar modificador de solo lectura
        int aux = 0;
#pragma warning restore IDE0044 // Agregar modificador de solo lectura
        public View_UserProfile()
        {
            InitializeComponent();
            BindingContext = new UserProfileViewModel(Navigation);
            aux = 1;
        }
        public View_UserProfile(UserModel user)
        {
            InitializeComponent();
            UserProfileViewModel upb = new UserProfileViewModel(user);
            BindingContext = upb;
            if (user.Type == 0)
            {
                contentPage.ToolbarItems.Add(new ToolbarItem()
                {
                    Order = ToolbarItemOrder.Secondary,
                    Text = "Establecer como adminsitrador",
                    Command = upb.SetAdminCommand
                });
            }
            else if(user.Type == 1)
            {
                contentPage.ToolbarItems.Add(new ToolbarItem()
                {
                    Order = ToolbarItemOrder.Secondary,
                    Text = "Remover Administrador",
                    Command = upb.RemoveAdminCommand
                });
            }

            if (user.Type != 2)
            {
                contentPage.ToolbarItems.Add(new ToolbarItem()
                {
                    Order = ToolbarItemOrder.Secondary,
                    Text = "Expulsar de la OTB",
                    Command = upb.RemoveOTBCommand
                });
            }

            aux = 0;
        }
        public View_UserProfile(string name,int phone,Guid id)
        {
            InitializeComponent();
            BindingContext = new UserProfileViewModel(name,phone,id);
            aux = 0;
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (aux==1)
            {
                BindingContext = new UserProfileViewModel(Navigation);
            }
        }
    }
}