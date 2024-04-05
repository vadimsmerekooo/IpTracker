using IpTracker.Models;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
                var regex = Regex.Matches(ipLine, ipRegex);
                if (regex.Count > 0 && System.Net.IPAddress.TryParse(regex[0].ToString(), out IPAddress ip))
                {
                    regex = Regex.Matches(ipLine, dateRegex);
                    if(regex.Count > 0 && DateTime.TryParse(regex[0].ToString(), out DateTime dateTime))
                    {
                        if(ipList.Any(ipe => ipe.Ip.Equals(ip)))
                        {
                            ipList.FirstOrDefault(ipe => ipe.Ip.Equals(ip)).Add(dateTime);
                        }
                        else
                        {
                            ipList.Add(new IpAdress(ip, new HashSet<DateTime>() { dateTime }));
                        }
                    }
                }
            }
            return ipList;
        }
        public HashSet<IpAdress> SortIpListConfig()
        {
            HashSet<IPAddress> ipList = new HashSet<IPAddress>();
            foreach (var ipItem in Config.IpAdressList)
            {
                IPAddress mask;
                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                    foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                        if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                            if (ipItem.Equals(unicastIPAddressInformation.Address))
                                mask = unicastIPAddressInformation.IPv4Mask;
                if (ipItem.DateTime.Any(d => d >= Config._timeStart && d <= Config._timeStart))
                {

                }

            }




            return null;
        }
    }
}
