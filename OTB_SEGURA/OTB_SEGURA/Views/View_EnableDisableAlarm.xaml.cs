using OTB_SEGURA.Models;
using OTB_SEGURA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_EnableDisableAlarm : ContentPage
    {
        public View_EnableDisableAlarm()
        {
            InitializeComponent();
        }
        public View_EnableDisableAlarm(AlarmModel alarma)
        {
            InitializeComponent();
            BindingContext = new ViewModel_EnableDisableAlarm(alarma);
        }
    }
}