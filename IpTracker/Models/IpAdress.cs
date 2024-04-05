
namespace IpTracker.Models
{
    internal class IpAdress
    {
        public IpAdress(System.Net.IPAddress ip, DateTime dateTime)
        {
            this.Ip = ip;
            this.DateTime = dateTime;
        }

        public System.Net.IPAddress Ip { get; }
        public DateTime DateTime { get; }
    }
}
