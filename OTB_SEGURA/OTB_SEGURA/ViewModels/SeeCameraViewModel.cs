using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;
using Amazon.KinesisVideo;
using Amazon.KinesisVideo.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OTB_SEGURA.ViewModels
{
    class SeeCameraViewModel : BaseViewModel
    {
        AmazonKinesisVideoClient client = new AmazonKinesisVideoClient("AKIAQPXAFAEE2CTZI4VA", "8i0yx+xRpH3P32QrwyeAGLttAvEhGvDqeqU+gP3o", Amazon.RegionEndpoint.SAEast1);
        List<Task<GetDataEndpointResponse>> respuestas = new List<Task<GetDataEndpointResponse>>();
        public ICommand SeeCamera
        {
            get
            {
                return new RelayCommand(async() => {
                    GetDataEndpointRequest getDataEndpointRequest=new GetDataEndpointRequest();
                    getDataEndpointRequest.StreamARN = "arn:aws:kinesisvideo:sa-east-1:033756086537:stream/CameraStream/1634161194699";
                    getDataEndpointRequest.StreamName = "CameraStream";
                    getDataEndpointRequest.APIName = APIName.GET_MEDIA;
                    await Task.Run(() => {
                        respuestas.Add(client.GetDataEndpointAsync(getDataEndpointRequest));
                    }).ConfigureAwait(false);
                    
                
                });
                
            }

        }
        public SeeCameraViewModel()
        {
            Title = "Ver Cámaras";
        }
    }
}
