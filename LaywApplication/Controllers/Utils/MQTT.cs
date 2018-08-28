using LaywApplication.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace LaywApplication.Controllers.Utils
{
    public class MQTT
    {
        readonly static MqttClient client = new MqttClient("m14.cloudmqtt.com", 12804, false, null, null, 0, null, null);
        public static void Listen(string topic, Action<object, MqttMsgPublishEventArgs> receivedMessage)
        {
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            byte clientId = client.Connect(Guid.NewGuid().ToString(), "yiutnzag", "xybyYIM0Hfk7");

            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }
        public static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //var message = System.Text.Encoding.Default.GetString(e.Message);
            //System.Console.WriteLine("Message received: " + message);
            Console.WriteLine(Encoding.UTF8.GetString(e.Message));
        }
    }
}
