using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using Xamarin.Forms;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
using Xamarin.Essentials;

namespace OTB_SEGURA.ViewModels
{
    /// <summary>
    /// Clase UserProfileViewModel que nos sirbe de logica para la vista View_UserProfile
    /// </summary>
    public class UserProfileViewModel:BaseViewModel
    {
        #region Attributes
        private UserModel user;
        private string textButton;
        FireBaseHelper firebaseHelper=new FireBaseHelper();
        private List<ActivityModel> activityList = new List<ActivityModel>();
        #endregion
        #region Properties
        public List<ActivityModel> ActivityList
        {
            get { return activityList; }
            set { activityList = value; OnPropertyChanged(); }
        }
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

        /// <summary>
        /// Constructor que recibe un objeto del tipo UserModel.
        /// Este constructor nos sirbe para dirigirnos a un perfil con la informacion del usuario recibido por parametro
        /// y podamos habilitar o deshabilitar a dicho usuario
        /// </summary>
        /// <param name="user"></param>
        public UserProfileViewModel(UserModel user)
        {          
            this.user = user;
            SetTextButton();
            LoadActivities(user.UserId.ToString());
            ButtonChangeStateClick = new Command(UpdateMethod);
        }
        /// <summary>
        /// Constructor que recibe como parametro un elemento del tipo INavigation
        /// </summary>
        /// <param name="navigation">parametro que nos sirbe para hacer redirecciones a otras vistas</param>
        public UserProfileViewModel(INavigation navigation)
        {
            user = new UserModel();
            textButton="Editar Mi Perfil";
            user.Name = Application.Current.Properties["Name"].ToString();
            user.UserName = Application.Current.Properties["UserName"].ToString();
            //user.Name = FireBaseHelper.staticUser.Name;
            //user.UserName = FireBaseHelper.staticUser.UserName;
            LoadActivities(Application.Current.Properties["Id"].ToString());
            //LoadActivities(FireBaseHelper.staticUser.UserId.ToString());
            ButtonChangeStateClick = new Command(async()=> 
            {
                await navigation.PushAsync(new View_Account());
            });
        }
        /// <summary>
        /// Constructor que nos sirbe para modificar el perfil con la informacion recibida por parametros
        /// </summary>
        /// <param name="name">parametro que nos sirbe para mostrar el nombre del usuario en el perfil</param>
        /// <param name="phone">parametro que nos sirbe para poder hacerle llamadas al numero recibido mediante este parametro</param>
        /// <param name="id">parametro que nos sirbe para poder hacer consultas a la bdd</param>
        public UserProfileViewModel(string name,int phone,Guid id)
        {
            user = new UserModel();
            user.Name = name;
            user.UserName = phone.ToString();
            user.UserId = id;
            LoadActivities(user.UserId.ToString());
            textButton = "LLamar";
            ButtonChangeStateClick = new Command(async () => {
                var answer = await App.Current.MainPage.DisplayAlert("Llamar a "+user.Name , "¿Desea realizar la llamada?", "Aceptar", "Cancelar");
                if (answer)
                {
                    PhoneDialer.Open(phone.ToString());
                }
            });

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
        /// <summary>
        /// metodo que nos sirbe para cambiar el texto del boton en la interfaz View_UserProfile
        /// </summary>
        private void SetTextButton()
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
        /// <summary>
        /// Metodo que actualiza el estado en bdd del usuario al que seleccionamos
        /// si el usuario esta inactivo el metodo lo pone activo
        /// si el usuario esta activo el metodo lo pone activo
        /// </summary>
        private async void UpdateMethod()
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
        /// <summary>
        /// Metodo que carga las actividades en la vista View_UserProfile segun el id del usuario que reciba
        /// </summary>
        /// <param name="id">codigo id de un usuario que sera utilizado para hacer consultas a la bdd</param>
        private async void LoadActivities(string id)
        {
            ActivityList = await firebaseHelper.GetAllActivitiesId(id);
        }
        #endregion
    }
}
