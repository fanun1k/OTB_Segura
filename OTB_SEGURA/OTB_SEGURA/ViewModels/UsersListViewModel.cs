using OTB_SEGURA.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace OTB_SEGURA.ViewModels
{
    public class UsersListViewModel : BaseViewModel
    {
        public ObservableCollection<UserModel> usersList {get;}

        public UsersListViewModel()
        {
            Title = "Lista de Usuarios";
            usersList = new ObservableCollection<UserModel>();
            usersList.Add(new UserModel {Name="Juan Perez Montoya",Phone=77889910,State=1 });
            usersList.Add(new UserModel { Name = "Alex Martinez", Phone = 72345678, State = 1 });
            usersList.Add(new UserModel { Name = "Roberto Zarate", Phone = 67456321, State = 1 });
        }
    }
}
