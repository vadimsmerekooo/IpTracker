using IpTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IpTracker.Service
{
    internal static class Config
    {
        public static HashSet<IpAdress> IpAdressList { get; set; }
        public static string _fileLogPath { get; set; }
        public static string _fileOutputPath { get; set; }
        public static IPAddress _adressStart { get; set; }
        public static IPAddress _adressMask { get;set; }
        public static DateTime _timeStart { get; set; }
        public static DateTime _timeEnd { get; set; }
        
    }
}
