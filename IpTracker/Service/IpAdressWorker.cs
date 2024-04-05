using IpTracker.Models;
using System.Text.RegularExpressions;

namespace IpTracker.Service
{
    internal class IpAdressWorker
    {
        private readonly string ipRegex = "\\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\b";
        private readonly string dateRegex = "[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) (2[0-3]|[01][0-9]):[0-5][0-9]:[0-5][0-9]";
        public HashSet<IpAdress> GetIpList(string[] readText)
        {
            HashSet<IpAdress> ipList = new HashSet<IpAdress>();
            foreach (var ipLine in readText)
            {
                System.Net.IPAddress ip;
                DateTime dateTime;
                var regex = Regex.Matches(ipLine, ipRegex);
                if (regex.Count > 0 && System.Net.IPAddress.TryParse(regex[0].ToString(), out ip))
                {
                    regex = Regex.Matches(ipLine, dateRegex);
                    if(regex.Count > 0 && DateTime.TryParse(regex[0].ToString(), out dateTime))
                    {
                        ipList.Add(new IpAdress(ip, dateTime));
                    }
                }
            }
            return ipList;
        }
    }
}
