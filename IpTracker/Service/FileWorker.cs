
using IpTracker.Models;
using System.Net;

namespace IpTracker.Service
{
    internal class FileWorker : IFile
    {
        public bool Exist(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception($"Not correct enter path.");
            }
            path = path.Trim().Replace("\"", "");
            if (File.Exists(path))
            {
                return true;
            }
            throw new Exception($"Path: {path} not found.");
        }
        public bool ExistDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception($"Not correct enter path.");
            }
            path = path.Trim().Replace("\"", "");
            if (Directory.Exists(path))
            {
                return true;
            }
            throw new Exception($"Directory: {path} not found.");
        }

        public HashSet<IpAdress> Read(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception($"Not correct enter path.");
            }
            path = path.Trim().Replace("\"", "");
            if (Exist(path) is bool)
            {
                string[] readText = File.ReadAllLines(path);
                if(readText.Length > 0)
                {
                    return new IpAdressWorker().GetIpList(readText);
                }
            }
            throw new Exception($"Error read file: {path} - not found.");
        }
        public bool ReadConfig(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception($"Not correct enter path.");
            }
            path = path.Trim().Replace("\"", "");
            if (Exist(path) is bool)
            {
                string[] readText = File.ReadAllLines(path);
                if (readText.Length > 0)
                {
                    foreach (var configItem in readText)
                    {
                        var _configItem = configItem.Trim().Split(":");
                        var _configItemValue = configItem.Replace($"{_configItem[0]}:", "").Trim();
                        switch(_configItem[0].Trim())
                        {
                            case "--file-log":
                                if (!String.IsNullOrEmpty(_configItemValue) && Exist(_configItemValue))
                                {
                                    Config._fileLogPath = _configItemValue;
                                }
                                break;
                            case "--file-output":
                                if (!String.IsNullOrEmpty(_configItemValue) && ExistDirectory(_configItemValue))
                                {
                                    Config._fileOutputPath = _configItemValue;
                                }
                                break;
                            case "--address-start":
                                if (!String.IsNullOrEmpty(_configItemValue) && IPAddress.TryParse(_configItemValue, out IPAddress ip))
                                {
                                    Config._adressStart = ip;
                                }
                                break;
                            case "--address-mask":
                                if (Config._adressStart != null && !String.IsNullOrEmpty(_configItemValue) && IPAddress.TryParse(_configItemValue, out IPAddress ipMask))
                                {
                                    Config._adressMask = ipMask;
                                }
                                break;
                            case "--time-start":
                                if (!String.IsNullOrEmpty(_configItemValue) && DateTime.TryParse(_configItemValue, out DateTime dateTime))
                                {
                                    Config._timeStart = dateTime;
                                }
                                break;
                            case "--time-end":
                                if (Config._timeStart != DateTime.MinValue && !String.IsNullOrEmpty(_configItemValue) && DateTime.TryParse(_configItemValue, out dateTime))
                                {
                                    if(Config._timeStart <= dateTime)
                                    {
                                        Config._timeEnd = dateTime;
                                    }
                                }
                                break;
                        }
                    };
                    return true;
                }
            }
            throw new Exception($"Error read file config: {path} - not found.");
        }

        public bool Write(string path, HashSet<IpAdress> ipAddresses)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception($"Not correct out path folder.");
            }
            path = path.Trim().Replace("\"", "");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine($"Parametrs: \n --address-start => {Config._adressStart}\n  --address-mask => {Config._adressMask}\n --time-start => {Config._timeStart} \n --time-end => {Config._timeEnd}\n __________________");
                foreach (var ip in ipAddresses)
                {
                    string dateTimes = string.Empty;
                    foreach (var ipDTC in ip.DateTime)
                    {
                        dateTimes += $"{ipDTC:yyyy-MM-dd HH:mm:ss},";
                    }
                    sw.WriteLine($"{ip.Ip}: Count connect -> {ip.DateTime.Count}, DateTime -> {dateTimes}");
                }
            }
            return true;
        }
    }
}
