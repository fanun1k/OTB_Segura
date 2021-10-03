using GalaSoft.MvvmLight.Command;
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
    public class EmergencyNumberUsersViewModel : BaseViewModel
    {
        #region Attributes
        FireBaseHelper firebaseHelper = new FireBaseHelper();
        private List<UserModel> userList; // Lista de usuarios registrados
        private UserService userService { get; set; } = new UserService();
        #endregion
        #region Properties
        public List<UserModel> UserList
        {
            get { return userList; }
            set { userList = value; OnPropertyChanged(); }
        }
        #endregion
        #region Construct
        public EmergencyNumberUsersViewModel()
        {
            Title = "Lista de Vecinos"; // Titulo de View_EmergencyNumberUserList
        }
        #endregion
        #region Commands
        public ICommand ItemTappedCommand { get; protected set; }

        public ICommand AppearingCommand // Comando para listar usuarios
        {
            get
            {
                return new RelayCommand(LoadData);
            }
        }
        public ICommand RefreshingCommand // Comando para refrescar la lista
        {
            get
            {
                return new RelayCommand(LoadData);
            }
        }
        #endregion        
        #region Methods
        public async void LoadData()
        {
            try
            {
                int otbID = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    ResponseHTTP<UserModel> responseHTTP = await userService.UsersByOtb(otbID);
                    if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                    {
                        UserList = responseHTTP.Data;
                        await App.SQLiteDB.SaveUserAsync(UserList);

                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);

                    }
                }
                else
                {
                    UserList = await App.SQLiteDB.GetUserAsync();

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
