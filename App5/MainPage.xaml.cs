using NModbus;
using NModbus.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace App5
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        static IModbusMaster master;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string deviceQuery = SerialDevice.GetDeviceSelector();
               
                var deviceInfo = await DeviceInformation.FindAllAsync(deviceQuery);

                if (deviceInfo != null && deviceInfo.Count > 0)
                {
                    

                    var serialBoardName = "COM1";
                   
                    var zbInfo = deviceInfo.Where(x => x.Name.Contains(serialBoardName)).First();
                    

                    SerialDevice serial;

                    var timer = Stopwatch.StartNew();
                    int attempts = 0;
                    do
                    {
                        serial = await SerialDevice.FromIdAsync(zbInfo.Id);
                        attempts++;
                        if (serial != null)
                            break;
                        await Task.Delay(100); // pause between retries
                    } while (serial == null && timer.ElapsedMilliseconds < 10000 && attempts < 5);
                    // max wait 10 seconds, or 5 total retries for serial device coms to come online.
                    timer.Stop();

                    

                   

                    serial.WriteTimeout = TimeSpan.FromMilliseconds(3000);
                    serial.ReadTimeout = TimeSpan.FromMilliseconds(3000);
                    serial.BaudRate = 115200;
                    serial.Parity = SerialParity.None;
                    serial.StopBits = SerialStopBitCount.One;
                    serial.DataBits = 8;
                    serial.Handshake = SerialHandshake.None;



                    SerialDeviceAdapter sd = new SerialDeviceAdapter(serial); ;

                    var factory = new ModbusFactory();

                    master = factory.CreateRtuMaster(sd);

                    master.Transport.ReadTimeout = 3000;
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Create a MessageDialog
            var dialog = new MessageDialog("This is my content", "");

            // If you want to add custom buttons
            dialog.Commands.Add(new UICommand("Click me!", delegate (IUICommand command)
            {
                // Your command action here
            }));


            dialog.Commands.Add(new UICommand("Click me more!", delegate (IUICommand command)
            {
                // Your command action here
            }));

            // Show dialog and save result
            var result = await dialog.ShowAsync();

            Console.WriteLine("trth");
        }
    }
}
