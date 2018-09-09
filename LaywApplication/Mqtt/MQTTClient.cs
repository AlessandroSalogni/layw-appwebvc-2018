using System;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace LaywApplication.Mqtt
{
    public class MQTTClient
    {
        private readonly MqttClient client;

        public MQTTClient(MQTTInfo MQTTConfig)
        {
            client = new MqttClient(MQTTConfig.BrokerHostName, MQTTConfig.BrokerPort, false, null, null, 0, null, null);
            client.Connect(Guid.NewGuid().ToString(), MQTTConfig.Username, MQTTConfig.Password);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
        }

        public void AddTopic(string topic)
            => client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        public void RemoveTopic(string topic) => client.Unsubscribe(new string[] { topic });

        public static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var splitTopic = e.Topic.Split(new char[] { '/' });

            var weightJson = (JObject)JObject.Parse(System.Text.Encoding.UTF8.GetString(e.Message))["weights"];
            weightJson.Remove("date");
            APIUtils.Post("https://localhost:44333/dashboard/mqtt/" + splitTopic[2] + "?patientId=" 
                + splitTopic[3] + "&doctorEmail=" + splitTopic[1], weightJson.ToString());
        }
    }
}