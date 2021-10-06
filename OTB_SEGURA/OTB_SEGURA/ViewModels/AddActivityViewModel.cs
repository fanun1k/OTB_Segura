using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    class AddActivityViewModel : BaseViewModel
    {
        #region Attributes
        private AlertTypeService alertTypeService = new AlertTypeService(); //servicio de tipos de alertas
        private AlertService alertService = new AlertService(); //servicio de alertas
        private List<AlertTypeModel> listAlertsType;
        private int selectedIndex;
        private AlertModel alerta = new AlertModel();  
        private AlertTypeModel alertTypeSelected;
        #endregion
        #region Properties
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; OnPropertyChanged(); }
        }
        public AlertTypeModel AlertTypeSelected
        {
            get { return alertTypeSelected; }
            set { alertTypeSelected = value; }
        }
        public AlertModel Alerta
        {
            get { return alerta; }
            set { alerta = value; OnPropertyChanged(); }
        }   
        public List<AlertTypeModel> ListAlertsType
        {
            get { return listAlertsType; }
            set { listAlertsType = value; OnPropertyChanged(); }
        }
        #endregion
        #region Constructor
        public AddActivityViewModel()
        {
            Title = "Alertar Evento";//Titulo de la vista
        }
        #endregion
        #region Command
        public ICommand AppearingCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await LoadAlertsType();
                });
            }
        }

        public ICommand SelectedIndexChangedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    try
                    {
                        if (alertTypeSelected != null)
                        {
                            await GetLocation();
                            var notify = await App.Current.MainPage.DisplayAlert("Enviar Alerta", $"Mensaje:{alerta.Message} \n" +
                                                                $"Tipo de alerta: {alertTypeSelected.Name} \n" +
                                                                $"Ubicación: {alerta.Latitude},{alerta.Longitude}\n" +
                                                                $"fecha: {DateTime.Now}", "Enviar", "Cancelar");
                            if (notify)
                            {
                                alerta.User_ID = int.Parse(Application.Current.Properties["User_ID"].ToString());
                                alerta.Date = DateTime.Now;
                                alerta.Otb_ID = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                                alerta.Alert_type_ID = alertTypeSelected.Alert_type_ID;
                                ResponseHTTP<AlertModel> responseHTTP = await alertService.insertarAlerta(Alerta);
                                DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                                PostNotification();
                            }
                            Alerta.Message = "";
                            Alerta = Alerta;
                            SelectedIndex = -1;
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }
                });
            }
        }
        //

        public ICommand EmergencyCommand//comando de boton de emergencia
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    //alerta = new AlertModel();
                    //await getLocation();//llamada a metodo para obtener ubicacion
                    //alerta.User_ID = int.Parse(Application.Current.Properties["User_ID"].ToString());
                    //alerta.Date = DateTime.Now;
                    //alerta.Otb_ID = int.Parse(Application.Current.Properties["Otb_ID"].ToString());
                    //alerta.Alert_type_ID = alertTypeSelected.Alert_type_ID;
                    //alerta.Message = "Alerta de Emergencia";
                    //ResponseHTTP<AlertModel> responseHTTP = await alertService.insertarAlerta(Alerta);
                    //DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                    //PostNotification();//llamada al metodo del envio de la notificacion});//referencia de metodo de creacion de la emergencia
                    DependencyService.Get<IMessage>().LongAlert("Falta reformular");
                });
                
        }   }
        #endregion
        #region Method
        //Nuevos meotodos
        private async void PostNotification()
        {//metodo de envio de notificaiciones
            try
            {
                //llave del servidor para peticiones http/post para generar notificaciones
                var serverKey = string.Format("key={0}", "AAAA4qfqqWY:APA91bGsZ0rQCczErCiSv-8-1bpO_U_Jmp1Q7-_GckSTf44jWe8MGKxQd0eJqb3IXXRJqQFsTaIW2POYZ_rjVrZrx492PmIyDGFjWaF3rmJ94IUVxRuce6KPf_TdTDngg7f0Hy35PWV_");
                var senderId = string.Format("id={0}", "973479782758");//id del sender da las notificaciones
                var data = new //datos de la notificaciones
                {
                    to = "/topics/all",//topic el que se mandara la notificacion
                    notification = new//nueva notificacion 
                    {
                        body = alerta.Message,//mensaje de la notificacion
                        title = alertTypeSelected.Name//titulo de la notificacion
                    }

                };
                var jsonBody = JsonConvert.SerializeObject(data);//generando json para las peticiones

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))//variable de envio de notificaciones
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);//definicion del header de autorizacion
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);//definicion del header del sender 
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");//definicion del contenido con json de la notificacion

                    using (var httpClient = new HttpClient())//variable de cliente de la peticion
                    {
                        var result = await httpClient.SendAsync(httpRequest);//definicion de variable de resultado de la peticion
                        if (!result.IsSuccessStatusCode)
                        {
                            DependencyService.Get<IMessage>().LongAlert("No se Pudo Notificar");//mensaje de error 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("Error: " + ex.Message);//mensaje en caso de error
            }
        }
        private async Task GetLocation()//metodo para obtener la ubicacion
        {
            await Task.Run(async()=>{
                var locator = CrossGeolocator.Current;//Nueva instancia para obtener ubicacion
                locator.DesiredAccuracy = 50;//definiendo la precision de la ubicacion
                var position = await locator.GetPositionAsync();
                alerta.Longitude = position.Longitude;
                alerta.Latitude = position.Latitude;
            });  
        }
        // Metodo de validacion de los Entry de la vista
        public bool ValidationEntry()
        {
            bool res = true;
            if (!alerta.Message.Equals(""))
            {
                if (!Regex.Match(alerta.Message, "^[ñA-Za-záéíóúÁÉÍÓÚ _]*[ñA-Za-záéíóúÁÉÍÓÚ][ñA-Za-záéíóúÁÉÍÓÚ _]*$").Success)
                {
                    res = false;
                    DependencyService.Get<IMessage>().LongAlert("Formato del mensaje incorrecto");
                }
            }
            else
            {
                res = false;
                DependencyService.Get<IMessage>().LongAlert("Por favor, introduzca una descripción del suceso");
            }

            return res;
        }
        //Metodos nuevos
        private async Task LoadAlertsType()
        {
            try
            {
                ResponseHTTP<AlertTypeModel> responseHTTP = await alertTypeService.GetAlertTypes();
                if (responseHTTP.Code == System.Net.HttpStatusCode.OK)
                {
                    ListAlertsType = responseHTTP.Data;
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert(responseHTTP.Msj);
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }
        }
        #endregion
    }
}
