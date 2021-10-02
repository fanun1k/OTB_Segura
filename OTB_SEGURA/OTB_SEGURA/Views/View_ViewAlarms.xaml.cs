using OTB_SEGURA.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OTB_SEGURA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View_ViewAlarms : ContentPage
    {
        public View_ViewAlarms()
        {
            InitializeComponent();
            BindingContext = new ViewModel_ViewAlarms(Navigation);
        }
    }
}