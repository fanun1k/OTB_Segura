using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace OTB_SEGURA.ViewModels
{
    public class EmergencyNumberUsersViewModel : BaseViewModel
    {
        #region Attributes
        FireBaseHelper firebaseHelper = new FireBaseHelper();
        private List<UserModel> userList; // Lista de usuarios registrados
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
            UserList = await firebaseHelper.GetActiveUsers(); //Obtiene la lista de usuarios activos en el sistema
        }

        #endregion
    }
}
