
using IpTracker.Models;
using System.Net;

namespace IpTracker.Service
{
    internal class FileWorker : IFile
    {
        public bool Exist(string path)
        {
            path = path.Trim().Replace("\"", "");
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception($"Not correct enter path.");
            }
            if (File.Exists(path))
            {
                return true;
            }
            throw new Exception($"Path: {path} not found.");
        }
        public bool ExistDirectory(string path)
        {
            path = path.Trim().Replace("\"", "");
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception($"Not correct enter path.");
            }
            if (Directory.Exists(path))
            {
                return true;
            }
            throw new Exception($"Directory: {path} not found.");
        }

        public HashSet<IpAdress> Read(string path)
        {
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
            path = path.Trim().Replace("\"", "");
            if (Exist(path) is bool)
            {
                string[] readText = File.ReadAllLines(path);
                if (readText.Length > 0)
                {
                    foreach (var configItem in readText)
                    {
                        var _configItem = configItem.Split(":");
                        var _configItemValue = configItem.Replace($"{_configItem[0]}:", "");
                        switch(_configItem[0])
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
                                IPAddress ip;
                                if (!String.IsNullOrEmpty(_configItemValue) && IPAddress.TryParse(_configItemValue, out ip))
                                {
                                    Config._adressStart = ip;
                                }
                                break;
                            case "--address-mask":
                                break;
                            case "--time-start":
                                DateTime dateTime;
                                if (!String.IsNullOrEmpty(_configItemValue) && DateTime.TryParse(_configItemValue, out dateTime))
                                {
                                    Config._timeStart = dateTime;
                                }
                                break;
                            case "--time-end":
                                if (!String.IsNullOrEmpty(_configItemValue) && DateTime.TryParse(_configItemValue, out dateTime))
                                {
                                    Config._timeStart = dateTime;
                                }
                                break;
                        }
                    };
                    return true;
                }
            }
            throw new Exception($"Error read file config: {path} - not found.");
        }

        public bool Write(string path, string text)
        {
            path = path.Trim().Replace("\"", "");
            throw new NotImplementedException();
        }
    }
}
