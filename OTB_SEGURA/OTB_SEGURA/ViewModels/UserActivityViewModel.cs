using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Text;
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
            LoadData();
        }
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
