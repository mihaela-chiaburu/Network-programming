using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Client.Models
{
    public class DnsResult
    {
        public bool IsSuccess { get; set; }
        public string[] Results { get; set; }
        public string ErrorMessage { get; set; }
        public DnsQueryType QueryType { get; set; }
        public string Query { get; set; }
        public string DnsServerUsed { get; set; }

        public DnsResult()
        {
            Results = new string[0];
        }

        public static DnsResult Success(string[] results, DnsQueryType queryType, string query, string dnsServer = null)
        {
            return new DnsResult
            {
                IsSuccess = true,
                Results = results,
                QueryType = queryType,
                Query = query,
                DnsServerUsed = dnsServer
            };
        }

        public static DnsResult Error(string errorMessage, DnsQueryType queryType, string query)
        {
            return new DnsResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                QueryType = queryType,
                Query = query
            };
        }
    }

    public enum DnsQueryType
    {
        Forward,   
        Reverse     
    }
}
