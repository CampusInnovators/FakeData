using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SendDataToIotHub
{
    class Sensor
    {
        private string _rawData;
        private string _sensorType;
        private string _timeStamp;
        private int _sampleRate;
        private string _sensorId;

        public Sensor()
        { }

        public Sensor(string sensorId, string rawData, string type, string timeStamp, int sampleRate)
        {
            SensorId = sensorId;
            RawData = rawData;
            SensorType = type;
            TimeStamp = timeStamp;
            SampleRate = sampleRate;
        }

        public string RawData
        {
            get
            {
                return _rawData;
            }

            set
            {
                _rawData = value;
            }
        }

        public string TimeStamp
        {
            get
            {
                return _timeStamp;
            }

            set
            {
                _timeStamp = value;
            }
        }

        public int SampleRate
        {
            get
            {
                return _sampleRate;
            }

            set
            {
                _sampleRate = value;
            }
        }

        public string SensorType
        {
            get
            {
                return _sensorType;
            }

            set
            {
                _sensorType = value;
            }
        }

        public string SensorId
        {
            get
            {
                return _sensorId;
            }

            set
            {
                _sensorId = value;
            }
        }
    }
}
    