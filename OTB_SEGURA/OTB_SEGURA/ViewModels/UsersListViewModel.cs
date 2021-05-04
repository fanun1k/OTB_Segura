using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    public class UsersListViewModel : BaseViewModel
    {

        FireBaseHelper firebaseHelper = new FireBaseHelper();
        public delegate void LoadDataDelegate();
        public LoadDataDelegate loadDataDelegate;
        private List<UserModel> userList;

        public List<UserModel> UserList
        {
            get { return userList; }
            set { userList = value;
                OnPropertyChanged();
            }
        }
        public UsersListViewModel()
        {
            Title = "Lista de Usuarios";
            loadDataDelegate = LoadData;
            loadDataDelegate();
        }
        #region Commands

        #endregion

        #region Metodh

        public async void LoadData()
        {
            UserList = await firebaseHelper.GetAllUsers();
            DependencyService.Get<IMessage>().ShortAlert(userList.Count.ToString());
        }

        #endregion
    }
}
