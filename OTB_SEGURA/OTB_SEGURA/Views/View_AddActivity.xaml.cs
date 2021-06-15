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
    public partial class View_AddActivity : ContentPage
    {
        public View_AddActivity()
        {
            InitializeComponent();
            if (btn1.IsEnabled == true)
            {
                btn1.IsEnabled = false;
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (check1.IsChecked == false)
            {
                btn1.IsEnabled = false;
            }
            else
            {
                btn1.IsEnabled = true;
            }
        }
    }
}