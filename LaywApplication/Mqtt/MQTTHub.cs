using LaywApplication.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace LaywApplication.Mqtt
{
    public class MQTTHub : Hub
    {
        private readonly IHubContext<MQTTHub> HubContext;

        public MQTTHub(IHubContext<MQTTHub> hubContext) => HubContext = hubContext;

        public void SendNotification(string message, string topic)
            => HubContext.Clients.All.SendAsync(topic, message);
    }
}
