using IpTracker.Service;
using System.Net;

bool showMenu = true;
while (showMenu)
{
    showMenu = MainMenu();
}

static bool MainMenu()
{
    bool showMenu = true;
    Console.Clear();
    Console.WriteLine("___IpTracker___");
    Console.Write($"Main menu options: \n 1. Load config file; \n 2. Set paramentrs in console; {(!Config.IsEmpty() ? $"\n 3. Config menu \n 4. Unload to file -> {Config.IpAdressList.Count} ip items" : "")} \n 0. Exit \n Enter options: ");
    switch (Console.ReadLine())
    {
        case "1":
            LoadConfigFile();
            return true;
        case "2":
            SetFileLogConfig();
            SetFileOutConfig();
            return true;
        case "3":
            if (!Config.IsEmpty())
            {
                while (showMenu)
                {
                    showMenu = ConfigMenu();
                }
            }
            return true;
        case "4":
            if (!Config.IsEmpty())
            {
                try
                {
                    FileWorker fileWorker = new FileWorker();
                    while (true)
                    {
                        Console.Write("Please enter name out file: ");
                        string nameOutFile = Console.ReadLine();
                        string path = $"{Config._fileOutputPath}\\{nameOutFile}";
                        WriteLine("Recording to the file has started, please wait.", ConsoleColor.Yellow);
                        if (fileWorker.Write(path, Config.IpAdressList))
                        {
                            Console.SetCursorPosition(0, Console.CursorTop - 1);
                            Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
                            WriteLine($"Sorted ip list success recording to file => {path}!\n Press key...", ConsoleColor.Green);
                            Console.ReadKey();
                            break;
                        }
                    }
                }
                catch(Exception ex)
                {
                    WriteLine(ex.Message, ConsoleColor.Red);
                    Console.ReadKey();
                }
            }
            return true;
        case "0":
            return false;
        default:
            return true;
    }
}

static bool ConfigMenu()
{
    Console.Clear();
    WriteLine("___IpTracker___");
    WriteLine("Warning. Attention, when clearing a parameter, enter 0!", ConsoleColor.Yellow);
    Console.Write($"Config menu options: \n 1. Change --file-output; \n 2. Change --address-start; \n 3. Change --address-mask; \n 4. Change --time-start; \n 5. Change --time-end; \n 6. Config parametrs; \n Count log ip addresses: {Config.IpAdressList.Count} \n 0. Return. \n Enter options: ");
    IpAdressWorker ipw = new IpAdressWorker();

    try
    {
        switch (Console.ReadLine())
        {
            case "1":
                return SetFileOutConfig();
            case "2":
                while (true)
                {
                    Console.Write("Please enter value for parametr --address-start (192.168.100.14): ");
                    var addressStart = Console.ReadLine();
                    if(addressStart == "0")
                    {
                        Config._adressStart = null;
                        WriteLine($"Parametr --address-start update success.\n Press key...", ConsoleColor.Green);
                        Console.ReadKey();
                        break;
                    }
                    if (System.Net.IPAddress.TryParse(addressStart, out IPAddress ipStart))
                    {
                        Config._adressStart = ipStart;
                        WriteLine($"Parametr --address-start {ipStart} update success.\n Press key...", ConsoleColor.Green);
                        Console.ReadKey();
                        break;
                    }
                    WriteLine($"Parametr --address-start {addressStart} update field. Not valid value.", ConsoleColor.Red);
                }
                ipw.UpdateIpListConfig();
                return true;
            case "3":
                while (true)
                {
                    if (Config._adressStart is null)
                    {
                        WriteLine($"Please set value for --address-start.\n Press key...", ConsoleColor.Red);
                        Console.ReadKey();
                        return true;
                    }
                    Console.Write("Please enter value for parametr --address-mask (192.168.100.255): ");
                    var addressMask = Console.ReadLine();
                    if(addressMask == "0")
                    {
                        Config._adressMask = null;
                        WriteLine($"Parametr --address-mask update success.\n Press key...", ConsoleColor.Green);
                        Console.ReadKey();
                        break;
                    }
                    if (System.Net.IPAddress.TryParse(addressMask, out IPAddress ipMask))
                    {
                        Config._adressMask = ipMask;
                        WriteLine($"Parametr --address-mask {ipMask} update success.\n Press key...", ConsoleColor.Green);
                        Console.ReadKey();
                        break;
                    }
                    WriteLine($"Parametr --address-mask {addressMask} update field. Not valid value.", ConsoleColor.Red);
                }
                ipw.UpdateIpListConfig();
                return true;
            case "4":
                while (true)
                {
                    Console.Write("Please enter value for parametr --time-start (dd.MM.yyyy): ");
                    var timeStart = Console.ReadLine();
                    if (timeStart == "0")
                    {
                        Config._timeStart = DateTime.MinValue;
                        Config._timeEnd = DateTime.MinValue;
                        WriteLine($"Parametr --time-start update success.\n Press key...", ConsoleColor.Green);
                        Console.ReadKey();
                        break;
                    }
                    if (DateTime.TryParse(timeStart, out DateTime dateTimeStart))
                    {
                        Config._timeStart = dateTimeStart;
                        WriteLine($"Parametr --time-start {dateTimeStart} update success.\n Press key...", ConsoleColor.Green);
                        Console.ReadKey();
                        break;
                    }
                    WriteLine($"Parametr --time-start {dateTimeStart} update field. Not valid value.", ConsoleColor.Red);

                }
                ipw.UpdateIpListConfig();
                return true;
            case "5":
                while (true)
                {
                    if (Config._timeStart == DateTime.MinValue)
                    {
                        WriteLine($"Please set value for --time-end.\n Press key...", ConsoleColor.Red);
                        Console.ReadKey();
                        return true;
                    }
                    Console.Write("Please enter value for parametr --time-end (dd.MM.yyyy): ");
                    var timeEnd = Console.ReadLine();
                    if (timeEnd == "0")
                    {
                        Config._timeEnd = DateTime.MinValue;
                        WriteLine($"Parametr --time-end update success.\n Press key...", ConsoleColor.Green);
                        Console.ReadKey();
                        break;
                    }
                    if (DateTime.TryParse(timeEnd, out DateTime dateTimeEnd) && Config._timeStart <= dateTimeEnd)
                    {
                        Config._timeEnd = dateTimeEnd;
                        WriteLine($"Parametr --time-end {dateTimeEnd} update success.\n Press key...", ConsoleColor.Green);
                        Console.ReadKey();
                        break;
                    }
                    WriteLine($"Parametr --time-end {dateTimeEnd} update field. Not valid value.", ConsoleColor.Red);

                }
                ipw.UpdateIpListConfig();
                return true;
            case "6":
                WriteLineConfig();
                return true;
            case "0":
                return false;
            default:
                return true;
        }
    }
    catch (Exception ex)
    {
        WriteLine(ex.Message, ConsoleColor.Red);
        Console.ReadKey();
        return true;
    }
}
static bool SetFileLogConfig()
{
    FileWorker fileWorker = new FileWorker();
    while (true)
    {
        Console.Write("\n--file-log: ");
        try
        {
            var _fileLog = Console.ReadLine();
            var resultFile = fileWorker.Exist(_fileLog);
            if (resultFile)
            {
                Config._fileLogPath = _fileLog;
                WriteLine($"Parametr _fileLog: {_fileLog} - set success.", ConsoleColor.Green);
                WriteLine($"File log read. Please wait...", ConsoleColor.Magenta);
                Config.IpAdressList = fileWorker.Read(_fileLog);
                if (Config.IpAdressList != null)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
                    WriteLine($"File log read success. ", ConsoleColor.Green);
                    if (Config.IpAdressList.Count > 0)
                    {
                        Config._timeStart = Config._timeStart == DateTime.MinValue ? Config.GetFirstDateTime() : Config._timeStart;
                        Config._timeEnd = Config._timeEnd == DateTime.MinValue ? Config.GetLastDateTime() : Config._timeEnd;
                        WriteLine($"Count log ip list: {Config.IpAdressList.Count}", ConsoleColor.Yellow);
                        Config.IpAdressList = new IpAdressWorker().SortIpListConfig();
                        WriteLine($"Count ip list sorted by parametrs: {Config.IpAdressList.Count}", ConsoleColor.Green);
                        break;
                    }
                    WriteLine($"Ip adress list count equals 0. Select another log file.", ConsoleColor.Yellow);
                }
            }
        }
        catch (Exception ex)
        {
            WriteLine(ex.Message, ConsoleColor.Red);
        }
    }
    return true;
}
static bool SetFileOutConfig()
{
    FileWorker fileWorker = new FileWorker();
    while (true)
    {
        Console.Write("\n--file-output: ");
        try
        {
            var _fileOutput = Console.ReadLine();
            var resultFile = fileWorker.ExistDirectory(_fileOutput);
            if (resultFile)
            {
                Config._fileOutputPath = _fileOutput;
                WriteLine($"Parametr _fileOutput: {_fileOutput} - set success.", ConsoleColor.Green);
                break;
            }
        }
        catch (Exception ex)
        {
            WriteLine(ex.Message, ConsoleColor.Red);
        }
    }
    return true;
}
static bool LoadConfigFile()
{
    FileWorker fileWorker = new FileWorker();
    while (true)
    {
        Console.Write("\nEnter path to file config: ");
        try
        {
            var _fileConfig = Console.ReadLine();
            var resultFile = fileWorker.ReadConfig(_fileConfig);
            if (resultFile)
            {
                WriteLine($"\n Load config file success. ", ConsoleColor.Green);
                WriteLine($"File log read. Please wait...", ConsoleColor.Magenta);
                Config.IpAdressList = fileWorker.Read(Config._fileLogPath);
                if (Config.IpAdressList != null)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
                    WriteLine($"File log read success. ", ConsoleColor.Green);
                    if (Config.IpAdressList.Count > 0)
                    {
                        Config._timeStart = Config._timeStart == DateTime.MinValue ? Config.GetFirstDateTime() : Config._timeStart;
                        Config._timeEnd = Config._timeEnd == DateTime.MinValue ? Config.GetLastDateTime() : Config._timeEnd;
                        WriteLine($"Count log ip list: {Config.IpAdressList.Count}", ConsoleColor.Yellow);
                        Config.IpAdressList = new IpAdressWorker().SortIpListConfig();
                        WriteLine($"Count ip list sorted by parametrs: {Config.IpAdressList.Count}", ConsoleColor.Green);
                    }
                    else
                    {
                        WriteLine($"Ip adress list count equals 0. Select another log file.", ConsoleColor.Yellow);
                    }
                }
                WriteLineConfig();
                break;
            }
        }
        catch (Exception ex)
        {
            WriteLine(ex.Message, ConsoleColor.Red);
        }
    }
    return true;
}


static void WriteLineConfig()
{
    WriteLine("Parametrs: ", ConsoleColor.Yellow);
    WriteLine($"{nameof(Config._fileLogPath)}: {(String.IsNullOrEmpty(Config._fileLogPath) ? "not value" : Config._fileLogPath)}");
    WriteLine($"{nameof(Config._fileOutputPath)}: {(String.IsNullOrEmpty(Config._fileOutputPath) ? "not value" : Config._fileOutputPath)}");
    WriteLine($"{nameof(Config._adressStart)}: {(Config._adressStart is null ? "not value" : Config._adressStart)}");
    WriteLine($"{nameof(Config._adressMask)}: {(Config._adressMask is null ? "not value" : Config._adressMask)}");
    WriteLine($"{nameof(Config._timeStart)}: {(Config._timeStart == DateTime.MinValue ? "not value" : Config._timeStart)}");
    WriteLine($"{nameof(Config._timeEnd)}: {(Config._timeStart == DateTime.MinValue ? "not value" : Config._timeEnd)}");
    WriteLine("Press key...");
    Console.ReadKey();
}
static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}
