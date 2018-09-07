using System;
using LaywApplication.Controllers;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace LaywApplication.Mqtt
{
    public class MQTTClient
    {
        private static MQTTClient instance;
        private static readonly object _sync = new object();
        private readonly MqttClient client = new MqttClient("m14.cloudmqtt.com", 12804, false, null, null, 0, null, null);

        private MQTTClient() { }

        public static MQTTClient Instance
        {
            get
            {
                if (instance == null)
                    lock (_sync) 
                        if (instance == null)
                        {
                            instance = new MQTTClient();
                            instance.client.Connect(Guid.NewGuid().ToString(), "yiutnzag", "xybyYIM0Hfk7");
                            instance.client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
                        }

                return instance;
            }
        }

        public void AddTopic(string topic)
            => client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        public void RemoveTopic(string topic) => client.Unsubscribe(new string[] { topic });

        public static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
            => new MQTTHub(HomeController.HubContext).SendNotification(sender.ToString(), e.Topic);
    }
}