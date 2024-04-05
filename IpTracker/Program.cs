using IpTracker.Service;

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
    Console.Write("Main menu options: \n 1. Load config file; \n 2. Set paramentrs in console; \n 0. Exit \n Enter options: ");
    switch (Console.ReadLine())
    {
        case "1":
            LoadConfigFile();
            while (showMenu)
            {
                showMenu = ConfigMenu();
            }
            return true;
        case "2":
            SetFileLogConfig();
            SetFileOutConfig();
            while (showMenu)
            {
                showMenu = ConfigMenu();
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
    Console.WriteLine("___IpTracker___");
    Console.Write("Config menu options: \n 1. Change --file-output; \n 2. Change --address-start; \n 3. Change --address-mask; \n 4. Change --time-start; \n 5. Change --time-end; \n 6. Config parametrs; \n 0. Return. \n Enter options: ");
    
    switch (Console.ReadLine())
    {
        case "1":
            return SetFileOutConfig();
        case "2":
            return true;
        case "3":
            return true;
        case "4":
            return true;
        case "5":
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
                        Config._timeStart = Config.IpAdressList.OrderBy(d => d.DateTime).First().DateTime;
                        Config._timeEnd = Config.IpAdressList.OrderByDescending(d => d.DateTime).First().DateTime;
                        WriteLine($"Count ip list: {Config.IpAdressList.Count}");
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
                WriteLine($"Load config file success. ", ConsoleColor.Green);
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
    WriteLine($"{nameof(Config._timeStart)}: {(Config._timeStart.Year == 0001 ? "not value" : Config._timeStart)}");
    WriteLine($"{nameof(Config._timeEnd)}: {(Config._timeStart.Year == 0001 ? "not value" : Config._timeEnd)}");
    WriteLine("Press key...");
    Console.ReadKey();
}
static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}
