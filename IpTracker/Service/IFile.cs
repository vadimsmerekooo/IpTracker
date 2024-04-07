using IpTracker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpTracker.Service
{
    internal interface IFile
    {
        public bool Write(string path, HashSet<IpAdress> ipAdresses);
        public HashSet<IpAdress> Read(string path);
        public bool Exist(string path);
    }
}
