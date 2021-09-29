using GalaSoft.MvvmLight.Command;
using OTB_SEGURA.Models;
using OTB_SEGURA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    /// <summary>
    /// Clase con informacion de numeros de emergencia estatica
    /// </summary>
    public class EmergencyNumbersViewModel:BaseViewModel
    {
        EmergencyNumbersViewModel e;
        public ObservableCollection<EmergencyNumbersModel> emergencyNumbersList { get; }
        public string Name { get; private set; }

     
        public EmergencyNumbersViewModel()
        {
            Title = "Numeros de emergencia";
            emergencyNumbersList = new ObservableCollection<EmergencyNumbersModel>();
            emergencyNumbersList.Add(new EmergencyNumbersModel { Name = "Radio patrullas", Number = 911, State = 1, Imagen = "policeCar.png"});
            emergencyNumbersList.Add(new EmergencyNumbersModel { Name = "Cuerpo de bomberos", Number = 119, State = 1, Imagen = "bom.png" });
            emergencyNumbersList.Add(new EmergencyNumbersModel { Name = "FELCC", Number = 4551690, State = 1, Imagen = "felcc.png" });
            emergencyNumbersList.Add(new EmergencyNumbersModel { Name = "SAR Bolivia", Number = 112, State = 1, Imagen = "sar.png" });
            emergencyNumbersList.Add(new EmergencyNumbersModel { Name = "Brigada Familiar", Number = 4233133, State = 1, Imagen = "family.png" });
            emergencyNumbersList.Add(new EmergencyNumbersModel { Name = "Medicar", Number = 161, State = 1, Imagen = "medicar.png" });
            emergencyNumbersList.Add(new EmergencyNumbersModel { Name = "Banco de sangre", Number = 4220229, State = 1, Imagen = "bloodBank.png" });
            emergencyNumbersList.Add(new EmergencyNumbersModel { Name = "ELFEC", Number = 4200125, State = 1, Imagen = "elfec.png" });
            emergencyNumbersList.Add(new EmergencyNumbersModel { Name = "SEMAPA", Number = 4290755, State = 1, Imagen = "semapa.png" });

            
            





        }
        


    }
}
