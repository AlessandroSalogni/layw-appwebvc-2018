using System;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Newtonsoft.Json.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace LaywApplication.Mqtt
{
    public class MQTTClient
    {
        private readonly MqttClient client;
        private readonly MQTTInfo MQTTConfig;
        private readonly QueryParams QueryParamsConfig;
        private readonly JsonData WeightConfig;
        private readonly ActivitySummary ActivitySummaryConfig;

        public MQTTClient(MQTTInfo MQTTConfig, JsonStructure jsonStructureConfig)
        {
            client = new MqttClient(MQTTConfig.BrokerHostName, MQTTConfig.BrokerPort, false, null, null, 0, null, null);
            client.Connect(Guid.NewGuid().ToString(), MQTTConfig.Username, MQTTConfig.Password);
            client.MqttMsgPublishReceived += MqttMsgPublishReceived;

            this.MQTTConfig = MQTTConfig;
            QueryParamsConfig = jsonStructureConfig.QueryParams;
            WeightConfig = jsonStructureConfig.Weight;
            ActivitySummaryConfig = jsonStructureConfig.ActivitySummary;
        }

        public void AddTopic(string topic)
            => client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        public void RemoveTopic(string topic) => client.Unsubscribe(new string[] { topic });

        public void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var splitTopic = e.Topic.Split(new char[] { '/' });
            if (splitTopic.Length < 4)
                return;

            var json = JObject.Parse(System.Text.Encoding.UTF8.GetString(e.Message));

            if (splitTopic[2] == WeightConfig.Key[0])
                json = (JObject)(json[WeightConfig.Key[0]] as JArray)[0];
            else if (splitTopic[2] == ActivitySummaryConfig.Root)
                json = (JObject)json[ActivitySummaryConfig.RootMQTT];

            json.Remove("date");
            APIUtils.Post(MQTTConfig.ControllerUrl + splitTopic[2].Replace("-", "") + "?" + 
                QueryParamsConfig.PatientId + "=" + splitTopic[3] + "&" + QueryParamsConfig.DoctorEmail + 
                "=" + splitTopic[1], json.ToString());
        }
    }
}