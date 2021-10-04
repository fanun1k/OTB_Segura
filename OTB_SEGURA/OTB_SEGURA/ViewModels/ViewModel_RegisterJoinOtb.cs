using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    public class ViewModel_RegisterJoinOtb : BaseViewModel
    {
        #region Attributes
        private OtbModel otb = new OtbModel();
        private OtbService otbService = new OtbService();
        public int User_ID { get; set; }
        #endregion

        #region Properties
        public OtbModel Otb
        {
            get { return otb; }
            set { otb = value; OnPropertyChanged(); }
        }
        #endregion
        #region Constructors
        public ViewModel_RegisterJoinOtb()
        {
            try
            {
                Title = "Registrar/Unirse a una OTB";
                User_ID = int.Parse(Application.Current.Properties["User_ID"].ToString());

            }
            catch (System.Exception ex)
            {

                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }

        }
        #endregion

        #region Commands
        public ICommand CreateOTBCommand
        {
            get
            {
                return new Command(async (obj) =>
                {
                    try
                    {
                        IsBusy = false;
                        ((Command)CreateOTBCommand).ChangeCanExecute();
                        Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
                        if (rg.IsMatch(otb.Name) && otb.Name.Length > 6)
                        {
                            ResponseHTTP<OtbModel> resultHTTP = await otbService.CreateOtb(otb.Name,User_ID);
                            if (resultHTTP.Code == System.Net.HttpStatusCode.OK)
                            {
                                await App.SQLiteDB.SaveMyOtb(otb);
                                Otb = resultHTTP.Data[0];
                                DependencyService.Get<IMessage>().LongAlert("OTB registrada con éxito");
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().LongAlert(resultHTTP.Msj);
                            }
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().LongAlert("El nombre de la OTB solo puede tener carácteres alfanuméricos y mayor a 6 letras");
                        }

                    }
                    catch (System.Exception ex)
                    {

                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                    IsBusy = true;
                }, canExecute: (obj) => { return IsBusy; });
            }
        }
        public ICommand JoinOtbCommand
        {
            get
            {
                return new Command(async (obj) =>
                {
                    try
                    {
                        IsBusy = false;
                        ((Command)JoinOtbCommand).ChangeCanExecute();
                        ResponseHTTP<OtbModel> responseHTTP = await otbService.JoinOtb(User_ID,otb.Code);
                        if (responseHTTP.Code==System.Net.HttpStatusCode.OK)
                        {
                            await App.SQLiteDB.SaveMyOtb(responseHTTP.Data[0]);
                            Application.Current.Properties["Otb_ID"] = responseHTTP.Data[0].Otb_ID;
                          
                            //aqui algun metodo para refresacar el menu principal con sus nuevas funcionalidades
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                        }
                    }
                    catch (System.Exception ex)
                    {

                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                    IsBusy = true;
                }, canExecute: (obj) => { return IsBusy; });
            }
        }
        #endregion

    }

}
