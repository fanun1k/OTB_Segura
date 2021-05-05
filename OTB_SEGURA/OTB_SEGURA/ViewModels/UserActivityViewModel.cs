using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    public class UserActivityViewModel:BaseViewModel
    {
        #region prop
        FireBaseHelper firebaseHelper = new FireBaseHelper();
        private List<UserActivityModel> listActivity=new List<UserActivityModel>();

        #endregion


        public List<UserActivityModel> ListActivity
        {
            get { return listActivity; }
            set { listActivity = value;
                OnPropertyChanged();
            }
        }

        #region Construct
        public UserActivityViewModel()
        {
            Title = "Actividad de Usuarios";
            LoadData();
        }
        #endregion

        #region Commands
        public ICommand ItemTappedCommandUserActivity { get; } = new Command(async (Item) =>
        {
            var userActivityModel = Item as UserActivityModel;
            if (userActivityModel != null)
            {
                await Map.OpenAsync(userActivityModel.Latitude, userActivityModel.Longitude, new MapLaunchOptions
                {
                    Name = "Ubicación",
                    NavigationMode = NavigationMode.None
                });
            }
        });
        #endregion

        #region Metodh

        public async void LoadData()
        {
            //await firebaseHelper.AddActivity(new UserActivityModel {Message="hola mundo",
            //Type="Alerta",
            //Latitude=123546,
            //Longitude=321456,
            //DateTime=DateTime.Now});
            //DependencyService.Get<IMessage>().ShortAlert("se inserto");
            ListActivity = await firebaseHelper.GetAllActivities();
        }
        #endregion
    }

}
