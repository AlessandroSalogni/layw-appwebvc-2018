using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
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
            APIUtils.Get("https://localhost:44333/dashboard/mqtt");
        }
    }

    public class MqttController : Controller
    {
        private readonly IHubContext<MqttHub> _hubContext;

        public MqttController(IHubContext<MqttHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("~/dashboard/mqtt")]
        public async Task<IActionResult> SendNotification()
        {
            await _hubContext.Clients.All.SendAsync("ciao", "ok");
            return Json(new { });
        }
    }

    public class MqttHub : Hub
    {
        public IHubContext<MqttHub> _context;

        public MqttHub(IHubContext<MqttHub> context)
        {
            _context = context;
        }

        public async Task SendMessage(string topic, string message)
        {
            await _context.Clients.All.SendAsync("ciao", message);
        }
    }
}
