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
    public partial class View_AlarmContainer : TabbedPage
    {
        public View_AlarmContainer()
        {
            InitializeComponent();
        }
        public View_AlarmContainer(List<string> listaAlertas)
        {
            InitializeComponent();

            foreach (var pestanhas in listaAlertas)
            {
                TappPrueba.Children.Add(new View_AlarmType(pestanhas));
            }
        }
    }
}