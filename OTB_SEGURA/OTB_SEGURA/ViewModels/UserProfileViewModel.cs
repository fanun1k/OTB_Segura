using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using Xamarin.Forms;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
namespace OTB_SEGURA.ViewModels
{
    class UserProfileViewModel:BaseViewModel
    {
        #region Attributes
        private UserModel user;
        private string textButton;
        FireBaseHelper firebaseHelper=new FireBaseHelper();
        #endregion
        #region Properties
        public string TextButton
        {
            get { return textButton; }
            set { textButton = value; OnPropertyChanged(); }
        }

        public UserModel User
        {
            get { return user; }
            set { user = value; }
        }
        #endregion
        #region Contructs
        public UserProfileViewModel(UserModel user)
        {          
            this.user = user;
            DependencyService.Get<IMessage>().ShortAlert(user.Name + "");
            SetTextButton();
            ButtonChangeStateClick = new Command(UpdateMethod);
        }
        public UserProfileViewModel(INavigation navigation)
        {
            user = new UserModel();
            textButton="Editar Mi Perfil";
            user.Name = Application.Current.Properties["Name"].ToString();
            user.UserName = Application.Current.Properties["UserName"].ToString();
            ButtonChangeStateClick = new Command(async()=> 
            {
                await navigation.PushAsync(new View_Account());
            });
        }
        public UserProfileViewModel(string name,int phone)
        {
            user = new UserModel();
            user.Name = name;
            user.UserName = phone.ToString();
            textButton = "LLamar";
        }
        #endregion
        #region Commands
        public ICommand ButtonChangeStateClick { get; private set; } 
        //{
        //    get
        //    {
        //        return new RelayCommand(UpdateMethod);
        //    }
        //}
          

        #endregion
        #region Methods
        public void SetTextButton()
        {
            switch (user.State)
            {
                case 1:
                    textButton = "Inhabilitar usuario";
                    break;
                case 0:
                    textButton = "habilitar/borrar usuario";
                    break;
            }
        }
        public async void UpdateMethod()
        {
            bool resDisplayAlert;
            if (user != null)
            {
                switch (user.State)
                {
                    //SI EL ESTADO ES =1 ENTONCES DEBEMOS DESHABILITAR AL USUARIO
                    case 1:
                        resDisplayAlert = await App.Current.MainPage.DisplayAlert("DESHABILITAR USUARIO", $"¿Esta seguro de deshabilitar al usuario: {user.Name}?", "Ok", "Cancelar");
                        if (resDisplayAlert)
                        {
                            await firebaseHelper.DisableUser(user);
                            DependencyService.Get<IMessage>().ShortAlert($"usuario {user.Name} deshabilitado");
                            SetTextButton();

                        }
                        break;
                    //SI ESTADO =2 ENTONCES DEBEMOS PREGUNTAR SI HABILITAR O ELIMINAR DEFINITIVAMENTE AL USUARIO
                    case 0:
                        resDisplayAlert = await App.Current.MainPage.DisplayAlert("HABILITAR O ELIMINAR USUARIO", $"¿Que desea hacer con el usuario: {user.UserName} ", "Habilitar", "Eliminar usuario");
                        if (resDisplayAlert)
                        {
                            await firebaseHelper.EnableUser(user);
                            DependencyService.Get<IMessage>().ShortAlert($"usuario {user.Name} habilitado");
                            SetTextButton();
                        }
                        else
                        {
                            await firebaseHelper.DeleteUser(user.UserId);
                            DependencyService.Get<IMessage>().ShortAlert($"usuario {user.Name} fue eliminado");
                            SetTextButton();
                        }
                        break;
                }

            }
            await Shell.Current.GoToAsync("..");
        }

        #endregion
    }
}
