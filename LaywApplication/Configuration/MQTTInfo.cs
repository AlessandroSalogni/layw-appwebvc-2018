namespace LaywApplication.Configuration
{
    public class MQTTInfo
    {
        public string BrokerHostName { get; set; }
        public int BrokerPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ControllerUrl { get; set; }
    }
}
