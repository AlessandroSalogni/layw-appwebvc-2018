using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Configuration
{
    public class ServerIP
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string Url { get; set; }

        public string GetTotalUrl()
        {
            return "http://" + IP + ":" + Port + Url;
        }
    }
}
