using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SendDataToIotHub
{
    public sealed partial class MainPage : Page
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "<HostNome>";
        static string deviceName = "<DeviceName>";
        static string deviceKey = "<DeviceKey>";

        private const int ledPin = 5;
        private const int pirPin = 6;

        private GpioPin led;
        private GpioPin pir;

        private DispatcherTimer timer;

        static private Sensor sensor1 = new Sensor("", "", "", DateTime.Now.ToString(), 0);
        static private Sensor[] sensors = new Sensor[Node.MAX_SENSORS];


        static private Node node1 = new Node("NodeId1", "ON_BOARD", sensors);
        static private Node[] nodes = new Node[Device.MAX_NODES];

        static private Device device1 = new Device("DeviceId1", "RP2", nodes);

        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            
            sensor1.SampleRate = 5;

            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceName, deviceKey), TransportType.Http1);
            InitGPIO();

            if (led != null)
            {
                timer.Start();
            }
        }

        private void InitGPIO()
        {
            // get the GPIO controller
            var gpio = GpioController.GetDefault();

            // return an error if there is no gpio controller
            if (gpio == null)
            {
                led = null;
                GpioStatus.Text = "There is no GPIO controller.";
                return;
            }

            // set up the LED on the defined GPIO pin
            // and set it to High to turn off the LED
            led = gpio.OpenPin(ledPin);
            led.Write(GpioPinValue.High);
            led.SetDriveMode(GpioPinDriveMode.Output);

            // set up the PIR sensor's signal on the defined GPIO pin
            // and set it's initial value to Low
            pir = gpio.OpenPin(pirPin);
            pir.SetDriveMode(GpioPinDriveMode.Input);

            //ID is the Pin number
            sensor1.SensorId = pir.PinNumber.ToString();
            sensor1.SensorType = "PIR";
            sensors[node1.CurrentSensor] = sensor1;

            GpioStatus.Text = "GPIO pins initialized correctly.";
        }

        private void Timer_Tick(object sender, object e)
        {
            TimeInterval.Text = DateTime.Now.ToString();
            sensor1.TimeStamp = DateTime.Now.ToString();

            // read the signal from the PIR sensor
            // if it is high, then motion was detected
            if (pir.Read() == GpioPinValue.High)
            {
                // turn on the LED
                led.Write(GpioPinValue.Low);

                // update the sensor status in the UI
                SensorStatus.Text = "Motion detected!";

                sensor1.RawData = "Occupied";

                SendDeviceToCloudMessagesAsync("occupied");
            }
            else
            {
                // turn off the LED
                led.Write(GpioPinValue.High);

                // update the sensor status in the UI
                SensorStatus.Text = "No motion detected.";

                sensor1.RawData = "Not Occupied";

                SendDeviceToCloudMessagesAsync("not occupied");
            }
        }

        private static async void SendDeviceToCloudMessagesAsync(string status)
        {
            //create anonymous types
            var sensorData = new
            {
                sensorID = sensor1.SensorId,
                sensorType = sensor1.SensorType,
                rawData = sensor1.RawData,
                timeStamp = sensor1.TimeStamp,
                sampleRate = sensor1.SampleRate
            };

            var nodeData = new
            {
                nodeID = node1.NodeID,
                nodeType = node1.NodeType,
            };

            var DeviceData = new
            {
                deviceID = device1.DeviceID,
                deviceType = device1.DeviceType
            };

            //concatenate types into messageString 
            var messageString = JsonConvert.SerializeObject(DeviceData);
            messageString += JsonConvert.SerializeObject(nodeData);
            messageString += JsonConvert.SerializeObject(sensorData);

            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message);
        }
    }
}
