using Confluent.Kafka;
using GalaSoft.MvvmLight.Command;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace OTB_SEGURA.ViewModels
{
    class SeeCameraViewModel : BaseViewModel
    {
        ConsumerConfig conf;
        private ImageSource imageVid;

        public ImageSource ImageVid
        {
            get { return imageVid; }
            set { imageVid = value;OnPropertyChanged(); }
        }



        public SeeCameraViewModel()
        {
            Title = "Ver Cámaras";

            conf = new ConsumerConfig
            {
                GroupId = "web_consumer",
                BootstrapServers = "192.168.0.100:29092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                //AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public ICommand SeeCamera
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
                        {
                            c.Subscribe("video-stream");

                            CancellationTokenSource cts = new CancellationTokenSource();
                            Console.CancelKeyPress += (_, e) =>
                            {
                                e.Cancel = true; // prevent the process from terminating.
                                cts.Cancel();
                            };
                            while (true)
                            {
                                try
                                {
                                    var cr = c.Consume(cts.Token);
                                    string key = cr.Message.Key == null ? "Null" : cr.Message.Key.ToString();
                                    //ImageVid = cr.Message.Value;  //esto puede ser......
                                    ImageVid = stringToImage(cr.Message.Value);
                                    //Console.WriteLine($"Consumed message '{cr.Message}' at: '{cr.TopicPartitionOffset}'.");
                                }
                                catch (ConsumeException e)
                                {
                                    throw e;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<IMessage>().LongAlert(ex.Message);
                    }              
                });
            
            }


        }
        public ImageSource stringToImage(string inputString)
        {
            byte[] imageBytes = Encoding.Unicode.GetBytes(inputString);

            // Don't need to use the constructor that takes the starting offset and length
            // as we're using the whole byte array.
            MemoryStream ms = new MemoryStream(imageBytes);

            ImageSource image = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));

            return image;
        }
    }
}
