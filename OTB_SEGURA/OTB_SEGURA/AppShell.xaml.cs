using OTB_SEGURA.ViewModels;

namespace OTB_SEGURA
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel(Navigation);
        }
    }
}
