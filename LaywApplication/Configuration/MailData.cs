using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Configuration
{
    public class MailData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int TimeOut { get; set; }
        public bool EnableSSL { get; set; }
        public string SMTP { get; set; }
    }
}