using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
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

        #region Attributes
        FireBaseHelper firebaseHelper = new FireBaseHelper();
        public INavigation Navigation { get; set; }
        private List<UserModel> userList;
        #endregion

        #region Properties
        public List<UserModel> UserList
        {
            get { return userList; }
            set { userList = value;
                OnPropertyChanged();
            }
        }
       
        #endregion

        #region Construct
        public UsersListViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Lista de Usuarios";         
            InitCommandTapped();
        }
        #endregion

        #region Commands   
        public ICommand ItemTappedCommand { get; protected set; }

        public ICommand AppearingCommand
        {
            get
            {
                return  new RelayCommand(LoadData);
            }
        }
        public ICommand RefreshingCommand
        {
            get
            {
                return new RelayCommand(LoadData);
            }
        } 
        #endregion

        #region Method

        /// <summary>
        /// this method obtains the data of all users
        /// </summary>
        public async void LoadData()
        {           
            UserList = await firebaseHelper.GetAllUsers();
            foreach (var user in userList)
            {
                if (user.State == 1) user.StateColor = "#dbeddc";
                else user.StateColor = "#ff9880";
            }
        }
        public void InitCommandTapped()
        {
            ItemTappedCommand = new Command(async (item) => {
                var user = item as UserModel;
                await Navigation.PushAsync(new View_UserProfile(user));
            });
        }
        #endregion
    }
}
