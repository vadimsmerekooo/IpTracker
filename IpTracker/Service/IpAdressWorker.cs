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
                    if (regex.Count > 0 && DateTime.TryParse(regex[0].ToString(), out DateTime dateTime))
                    {
                        if (ipList.Any(ipe => ipe.Ip.Equals(ip)))
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

        public void UpdateIpListConfig()
        {
            try
            {
                Config.IpAdressList = SortIpListConfig();
            }
            catch (Exception ex)
            {
                throw new Exception("Error any update ip list. Try again");
            }
        }

        public HashSet<IpAdress> SortIpListConfig()
        {
            Config.IpAdressList = new FileWorker().Read(Config._fileLogPath);
            HashSet<IpAdress> ipList = new HashSet<IpAdress>();

            Config._timeStart = Config._timeStart == DateTime.MinValue ? Config.GetFirstDateTime() : Config._timeStart;
            Config._timeEnd = Config._timeEnd == DateTime.MinValue ? Config.GetLastDateTime() : Config._timeEnd;

            foreach (var ipItem in Config.IpAdressList)
            {
                if (ipItem.DateTime.Any(d => d >= Config._timeStart && d <= Config._timeEnd) && IsInRange(ipItem.Ip, Config._adressStart, Config._adressMask))
                {
                    IpAdress ip = ipItem;
                    foreach (var ipDTC in ipItem.DateTime)
                    {
                        if (ipDTC < Config._timeStart || ipDTC > Config._timeEnd)
                            ip.DateTime.Remove(ipDTC);
                    }
                    ipList.Add(ipItem);
                }
            }
            return ipList;
        }
        bool IsInRange(IPAddress ip, IPAddress addressStart, IPAddress addressMask)
        {
            if (ip is null) return false;
            if (addressStart is null) return true;


            bool areEqual = false;
            byte[] ipBytes = ip.GetAddressBytes();
            byte[] ipAddressStartBytes = addressStart.GetAddressBytes();


            for (int i = 0; i < ipBytes.Length; i++)
            {
                if (addressMask is null)
                {
                    areEqual = ipBytes[i] >= ipAddressStartBytes[i];
                }
                else
                {
                    byte[] ipAddressMaskBytes = addressMask.GetAddressBytes();
                    areEqual = ipBytes[i] >= ipAddressStartBytes[i] && ipBytes[i] <= ipAddressMaskBytes[i];
                }
                if (!areEqual)
                    break;
            }
            return areEqual;
        }
    }
}
