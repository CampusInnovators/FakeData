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
    class Device
    {
        public const int MAX_NODES = 3;

        private string _deviceID;
        private string _deviceType;
        private Node[] _nodes = new Node[MAX_NODES];
        
        private int _currentNode;

        public Device()
        {
            CurrentNode = 0;
        }

        public Device(string did, string type, Node[] nodes)
        {
            CurrentNode = 0;
            DeviceID = did;
            DeviceType = type;
            Nodes = nodes;
        }

        public string DeviceID
        {
            get
            {
                return _deviceID;
            }

            set
            {
                _deviceID = value;
            }
        }

        public string DeviceType
        {
            get
            {
                return _deviceType;
            }

            set
            {
                _deviceType = value;
            }
        }

        internal Node[] Nodes
        {
            get
            {
                return _nodes;
            }

            set
            {
                _nodes = value;
            }
        }

        public int CurrentNode
        {
            get
            {
                return _currentNode;
            }

            set
            {
                _currentNode = value;
            }
        }

        public Node getNode(int index)
        {
            if (index > CurrentNode)
            {
                throw new System.ArgumentException("No Node in that index");
            }

            return _nodes[index];
        }

        public void addNode(Node node)
        {
            if (CurrentNode == MAX_NODES - 1)
            {
                throw new System.ArgumentException("Nodes at full capacity");
            }

            Nodes[CurrentNode] = node;
            CurrentNode++;
        }

        public void setNode(int index, Node node)
        {
            if (CurrentNode == MAX_NODES - 1)
            {
                throw new System.ArgumentException("Nodes at full capacity");
            }

            if(index > CurrentNode)
            {
                throw new System.ArgumentException("No Node in that index");
            }

            for(int i = 0; i < MAX_NODES; i++)
            {
                if(i == index)
                {
                    Nodes[i] = node;
                }
            }
        }
    }
}
