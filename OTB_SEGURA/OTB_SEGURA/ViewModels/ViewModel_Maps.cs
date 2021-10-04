using OTB_SEGURA.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace OTB_SEGURA.ViewModels
{
    class ViewModel_Maps : BaseViewModel
    {
        #region Attributes
        private CompleteAlertModel alert;
        private ObservableCollection<Pin> pinList = new ObservableCollection<Pin>();
        private Position position;
        #endregion

        #region Properties
        public Position Position
        {
            get { return position; }
            set { position = value; OnPropertyChanged(); }
        }
        public CompleteAlertModel Alert
        {
            get { return alert; }
            set { alert = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Pin> PinList 
        {
            get { return pinList; }
            set { pinList = value; OnPropertyChanged(); }
        }
        #endregion
        #region Constructor
        public ViewModel_Maps(CompleteAlertModel alerta)
        {
            Alert = alerta;
            try
            {
                if (alert.Ubication_List.Count > 0)
                {
                    
                    var query = from x in alert.Ubication_List
                                select new Pin { Position = new Position(x.Latitude, x.Longitude), Label ="" };
                    foreach (var pin in query)
                    {
                        PinList.Add(pin);
                    }
                    Position = PinList[0].Position;
                }
                else
                {
                    pinList.Add(new Pin {Position=new Position(alert.Latitude, alert.Longitude),Label=""});
                    Position = new Position(alert.Latitude, alert.Longitude);
                }
            }
            catch (System.Exception ex)
            {

                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
            
            
        }
        #endregion
    }
}
