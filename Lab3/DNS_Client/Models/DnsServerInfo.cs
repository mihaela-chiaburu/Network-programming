using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Client.Models
{
    public class DnsServerInfo
    {
        public string Address { get; set; }
        public bool IsReachable { get; set; }
        public string Name { get; set; }
        public DateTime LastChecked { get; set; }

        public DnsServerInfo(string address, string name = null)
        {
            Address = address;
            Name = name ?? address;
            LastChecked = DateTime.Now;
        }
    }
}
