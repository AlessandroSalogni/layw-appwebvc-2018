using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywAuthentication.Configuration
{
    public class HomestationIP
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string Url { get; set; }

        public string getTotalUrl()
        {
            return "http://" + IP + ":" + Port + Url; 
        }
    }
}
