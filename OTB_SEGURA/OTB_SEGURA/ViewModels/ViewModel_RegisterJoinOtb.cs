using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    public class ViewModel_RegisterJoinOtb : BaseViewModel
    {

        private OtbModel otb=new OtbModel();
        public OtbModel Otb
        {
            get { return otb; }
            set { otb = value; OnPropertyChanged(); }
        }


        public ICommand CreateOTBCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }

        public ViewModel_RegisterJoinOtb()
        {
            try
            {
                Title = "Registrar/Unirse a una OTB";
                Otb.User_ID = int.Parse(Application.Current.Properties["User_ID"].ToString());
                DependencyService.Get<IMessage>().LongAlert(Application.Current.Properties["UserName"] as string +" "+ Application.Current.Properties["Ci"] as string+ " "+ (Application.Current.Properties["Password"] as string+ Application.Current.Properties["Phone"] as string+" "+ Application.Current.Properties["UserType"] as string));
            }
            catch (System.Exception ex)
            {

                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
         
        }

    }

}
