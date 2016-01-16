using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendDataToIotHub
{
    class Node
    {
        public const int MAX_SENSORS = 10;

        private string _nodeID;
        private string _nodeType;
        private Sensor[] _sensors = new Sensor[MAX_SENSORS];
        private int _currentSensor = 0;

        public Node()
        { }

        public Node(string id, string type, Sensor[] sensors)
        {
            NodeID = id;
            NodeType = type;
            _sensors = sensors;
        }

        public string NodeID
        {
            get
            {
                return _nodeID;
            }

            set
            {
                _nodeID = value;
            }
        }

        public string NodeType
        {
            get
            {
                return _nodeType;
            }

            set
            {
                _nodeType = value;
            }
        }

        public int CurrentSensor
        {
            get
            {
                return _currentSensor;
            }

            set
            {
                _currentSensor = value;
            }
        }

        public Sensor getSensor()
        {
            return _sensors[CurrentSensor];
        }

        public Sensor getSensor(int index)
        {
            if (index > CurrentSensor)
            {
                throw new System.ArgumentException("No Sensor in that index");
            }

            return _sensors[index];
        }

        public void addSensor(Sensor sensor)
        {
            if (CurrentSensor == MAX_SENSORS - 1)
            {
                throw new System.ArgumentException("Sensors at full capacity");
            }

            _sensors[CurrentSensor] = sensor;
            CurrentSensor++;
        }

        public void setSensor(int index, Sensor sensor)
        {
            if (CurrentSensor == MAX_SENSORS - 1)
            {
                throw new System.ArgumentException("Sensors at full capacity");
            }

            if (index > CurrentSensor)
            {
                throw new System.ArgumentException("No Sensor in that index");
            }

            for (int i = 0; i < MAX_SENSORS; i++)
            {
                if (i == index)
                {
                    _sensors[i] = sensor;
                }
            }
        }
    }
}
