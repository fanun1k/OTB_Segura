using OTB_SEGURA.Models;
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
    public partial class View_UserAlarmTabbedPage : TabbedPage
    {
        public View_UserAlarmTabbedPage()
        {
            InitializeComponent();
            List<String> listaAlertas = new List<string>();
            listaAlertas.Add("General");
            listaAlertas.Add("Robos");
            listaAlertas.Add("Incendios");
            listaAlertas.Add("Accidentes");
            listaAlertas.Add("Rescates");
            foreach (var pestanhas in listaAlertas)
            {
                TappPrueba.Children.Add(new View_AlertType(pestanhas));
            }
        }
    }
}