using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.ViewModels
{
    public class EmergencyNumberUsersViewModel : BaseViewModel
    {
        #region Attributes
        FireBaseHelper firebaseHelper = new FireBaseHelper();
        private List<UserModel> userList;
        #endregion
        #region Properties
        public List<UserModel> UserList
        {
            get { return userList; }
            set { userList = value; OnPropertyChanged(); }
        }
        #endregion
        #region Construct
        public void UsersListViewModel()
        {
            Title = "Lista de Vecinos";

        }
        #endregion
        #region Methods
        public async void LoadData()
        {
            UserList = await firebaseHelper.GetAllUsers();
            foreach (var user in userList)
            {
                user.StateColor = (user.State == 1) ? "#dbeddc" : "#ff9880";
            }
        }
        #endregion
    }
}
