using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace LaywApplication.Controllers
{
    public class MQTTController : Controller
    {
        // create client instance
        MqttClient client = new MqttClient("tcp://localhost:1883");
        void Main()
        {
            // register to message received
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic 
            client.Subscribe(new string[] { "/server/s.coppeta11@gmail.com" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); //todo mettere email dottore
        }
        public static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received
        }
    }
}