using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using OTB_SEGURA.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    /// <summary>
    /// Clase UserProfileViewModel que nos sirbe de logica para la vista View_UserProfile
    /// </summary>
    public class UserProfileViewModel:BaseViewModel
    {
        #region Attributes
        private UserModel user = new UserModel();
        private string textButton;
        private List<AlertModel> activityList = new List<AlertModel>();
        private UserService restFull = new UserService();
        public ICommand ButtonChangeStateClick { get; private set; }
        private AlertService alertService= new AlertService();
        private bool editButtonVisivility=true;
        private ImageSource imgProfile;

        

        #endregion
        #region Properties
        public bool EditButtonVisivility
        {
            get { return editButtonVisivility; }
            set { editButtonVisivility = value; OnPropertyChanged(); }
        }
        public List<AlertModel> ActivityList
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
            set { user = value;OnPropertyChanged(); }
        }

        public ImageSource ImgProfile
        {
            get { return imgProfile; }
            set { imgProfile = value; OnPropertyChanged(); }
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
            LoadActivities();
            EditButtonVisivility = false;

        }
        /// <summary>
        /// Constructor que recibe como parametro un elemento del tipo INavigation
        /// </summary>
        /// <param name="navigation">parametro que nos sirbe para hacer redirecciones a otras vistas</param>
        public UserProfileViewModel(INavigation navigation)
        {
            try
            {
                user = new UserModel();
                textButton = "Editar Mi Perfil";
                user.Name = Application.Current.Properties["Name"].ToString();
                user.Email = Application.Current.Properties["Email"].ToString();
                user.User_ID = int.Parse(Application.Current.Properties["User_ID"].ToString());
                user.Otb_ID = int.Parse(Application.Current.Properties["Otb_ID"].ToString());

                LoadActivities();
                ButtonChangeStateClick = new Command(async () =>
                {
                    await navigation.PushAsync(new View_Account());
                });
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
           
        }
        /// <summary>
        /// Constructor que nos sirbe para modificar el perfil con la informacion recibida por parametros
        /// </summary>
        /// <param name="name">parametro que nos sirbe para mostrar el nombre del usuario en el perfil</param>
        /// <param name="phone">parametro que nos sirbe para poder hacerle llamadas al numero recibido mediante este parametro</param>
        /// <param name="id">parametro que nos sirbe para poder hacer consultas a la bdd</param>
        public UserProfileViewModel(string name,int phone,int user_id,Nullable<int> otb_id)
        {
            user = new UserModel
            {
                Name = name,
                Email = phone.ToString(),
                User_ID = user_id,
                Otb_ID=otb_id
            };
            LoadActivities();
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
        public ICommand SetAdminCommand
        {
            get 
            { 
                return new Command(execute: async(obj) => {
                    try
                    {
                        IsBusy = false;
                        ((Command)SetAdminCommand).ChangeCanExecute();
                        if (await App.Current.MainPage.DisplayAlert("Dar Administrador", "¿Desea volver administrador a "
                                                                    +$"{user.Name} ?", "Aceptar", "Cancelar"))
                        {
                            ResponseHTTP<UserModel> resultHTTP = await restFull.SetAdmin(user);

                            if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                                await Shell.Current.GoToAsync("..");
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                    IsBusy = true;
                },canExecute: (obj) => { return IsBusy; }); 
            }
        }

        public ICommand RemoveAdminCommand
        {
            get
            {
                return new Command(execute: async(obj) => {
                    try
                    {
                        IsBusy = false;
                        ((Command)RemoveAdminCommand).ChangeCanExecute();
                        if (await App.Current.MainPage.DisplayAlert("Remover Administrador", "¿Desea volver quitarle el Administrador a "
                                                                    + $"{user.Name} ?", "Aceptar", "Cancelar"))
                        {
                            ResponseHTTP<UserModel> resultHTTP = await restFull.RemoveAdmin(user);

                            if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                                await Shell.Current.GoToAsync("..");
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                    IsBusy = true;
                },canExecute: (obj) => { return IsBusy; });
            }
        }

        public ICommand RemoveOTBCommand
        {
            get
            {
                return new Command(execute: async (obj) => {
                    try
                    {
                        IsBusy = false;
                        ((Command)RemoveOTBCommand).ChangeCanExecute();
                        if (await App.Current.MainPage.DisplayAlert("Expulsar OTB", "¿Desea volver expulsar a "
                                                                    + $"{user.Name} ?", "Aceptar", "Cancelar"))
                        {
                            ResponseHTTP<UserModel> resultHTTP = await restFull.RemoveOTB(user);

                            if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                                await Shell.Current.GoToAsync("..");
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                    IsBusy = true;
                },canExecute: (obj) => { return IsBusy; });
            }
        }

        public ICommand UploadCommand
        {
            get
            {
                return new Command(async (obj) =>
                {
                    try
                    {
                        IsBusy = false;
                        ((Command)UploadCommand).ChangeCanExecute();
                        Stream stream = await DependencyService.Get<IOpenGalery>().GetFotoAsync();

                        if (stream != null)
                        {
                            MemoryStream ms = new MemoryStream();
                            stream.CopyTo(ms);

                            ResponseHTTP<UserModel> resultHTTP = await restFull.UploadProfile(user.User_ID.ToString(), new MemoryStream(ms.ToArray()));
                            ImgProfile = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
                            if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                    IsBusy = true;
                }, canExecute: (obj) => { return IsBusy; });
            }
        }
        public ICommand AppearingProfileCommand
        {
            get
            {
                return new RelayCommand(async () => {
                    await LoadImageProfile();
                });
            }
        }

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
            try
            {

                if (user != null)
                {
                    switch (user.State)
                    {
                        //SI EL ESTADO ES =1 ENTONCES DEBEMOS DESHABILITAR AL USUARIO
                        case 1:
                            resDisplayAlert = await App.Current.MainPage.DisplayAlert("DESHABILITAR USUARIO", $"¿Esta seguro de deshabilitar al usuario: {user.Name}?", "Ok", "Cancelar");
                            if (resDisplayAlert)
                            {
                                //await firebaseHelper.DisableUser(user);
                                DependencyService.Get<IMessage>().ShortAlert($"usuario {user.Name} deshabilitado");
                                SetTextButton();

                            }
                            break;
                        //SI ESTADO =2 ENTONCES DEBEMOS PREGUNTAR SI HABILITAR O ELIMINAR DEFINITIVAMENTE AL USUARIO
                        case 0:
                            resDisplayAlert = await App.Current.MainPage.DisplayAlert("HABILITAR O ELIMINAR De La OTB USUARIO", $"¿Que desea hacer con el usuario: {user.Name} ", "Habilitar", "Eliminar usuario");
                            if (resDisplayAlert)
                            {
                                //await firebaseHelper.EnableUser(user);
                                DependencyService.Get<IMessage>().ShortAlert($"usuario {user.Name} habilitado");
                                SetTextButton();
                            }
                            else
                            {
                                //await firebaseHelper.DeleteUser(user.UserId);
                                DependencyService.Get<IMessage>().ShortAlert($"usuario {user.Name} fue eliminado");
                                SetTextButton();
                            }
                            break;
                    }
                    await Shell.Current.GoToAsync("..");
                }
                }
            catch (Exception ex)
            {

                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
          

           
        }
        /// <summary>
        /// Metodo que carga las actividades en la vista View_UserProfile segun el id del usuario que reciba
        /// </summary>
        /// <param name="id">codigo id de un usuario que sera utilizado para hacer consultas a la bdd</param>
        /// 

        public async void LoadActivities()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    ResponseHTTP<AlertModel> responseHTTP = await alertService.GetAlertsByUser(user.Otb_ID, user.User_ID);
                    if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                    {
                        ActivityList = responseHTTP.Data;
                        await App.SQLiteDB.SaveAlertAsync(activityList);

                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);

                    }
                }
                else
                {
                    ActivityList = await App.SQLiteDB.GetAlertAsync();

                }
            }
            catch (System.Exception ex)
            {

                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
        }

        private async Task LoadImageProfile()
        {
            if (imgProfile == null)
            {
                User = await restFull.GetImageProfile(user);
                ImgProfile = User.Photo;
            }
        }

        #endregion
    }
}
