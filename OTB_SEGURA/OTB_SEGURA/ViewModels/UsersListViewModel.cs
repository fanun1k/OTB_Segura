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

        #region Attributes
        FireBaseHelper firebaseHelper = new FireBaseHelper();
      
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
        public UsersListViewModel()
        {
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
            ItemTappedCommand = new Command((item) => {
                UpdateMethod(item);
            });
        }
        public async void UpdateMethod(object item)
        {
            var userModel = item as UserModel;
            bool resDisplayAlert;
            if (userModel != null)
            {
                switch (userModel.State)
                {
                    //SI EL ESTADO ES =1 ENTONCES DEBEMOS DESHABILITAR AL USUARIO
                    case 1:
                        resDisplayAlert = await App.Current.MainPage.DisplayAlert("DESHABILITAR USUARIO", $"¿Esta seguro de deshabilitar al usuario: {userModel.Name}?", "Ok", "Cancelar");
                        if (resDisplayAlert)
                        {
                            await firebaseHelper.DisableUser(userModel);
                            DependencyService.Get<IMessage>().ShortAlert($"usuario {userModel.Name} deshabilitado");
                            LoadData();
                            
                        }
                        break;
                        //SI ESTADO =2 ENTONCES DEBEMOS PREGUNTAR SI HABILITAR O ELIMINAR DEFINITIVAMENTE AL USUARIO
                    case 0:
                        resDisplayAlert = await App.Current.MainPage.DisplayAlert("HABILITAR O ELIMINAR USUARIO", $"¿Que desea hacer con el usuario: {userModel.UserName} ", "Habilitar", "Eliminar usuario");
                        if (resDisplayAlert)
                        {
                            await firebaseHelper.EnableUser(userModel);
                            DependencyService.Get<IMessage>().ShortAlert($"usuario {userModel.Name} habilitado");
                            LoadData();
                        }
                        else
                        {
                            await firebaseHelper.DeleteUser(userModel.UserId);
                            DependencyService.Get<IMessage>().ShortAlert($"usuario {userModel.Name} fue eliminado");
                            LoadData();
                        }
                        break;                       
                }
                
            }
        }
        #endregion
    }
}
