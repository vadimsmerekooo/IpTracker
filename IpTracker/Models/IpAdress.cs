
namespace IpTracker.Models
{
    internal class IpAdress
    {
        public IpAdress(System.Net.IPAddress ip, HashSet<DateTime> dateTime)
        {
            this.Ip = ip;
            this.DateTime = dateTime;
            this.CountConnection = 1;
        }

        public System.Net.IPAddress Ip { get; }
        public HashSet<DateTime> DateTime { get; private set; }
        public int CountConnection { get; private set; }
        public void Add(DateTime dateTime)
        {
            this.DateTime.Add(dateTime);
            CountConnection++;
        }
        public DateTime GetFirstConnectDateTime()
        {
            return DateTime.OrderBy(dateTime => dateTime).FirstOrDefault();
        }
        public DateTime GetLastConnectDateTime()
        {
            return DateTime.OrderByDescending(dateTime => dateTime).FirstOrDefault();
        }
    }
}
