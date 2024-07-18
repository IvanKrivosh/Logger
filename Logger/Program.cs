using System;
using System.IO;

namespace Logger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DayOfWeek weekDay = DayOfWeek.Thursday;
            string message = "Some text";
            string fileName = "log.txt";
            FileLogWritter fileLogger = new FileLogWritter(fileName);
            ConsoleLogWritter consoleLogger = new ConsoleLogWritter();
            Pathfinder pathfinderFile = new Pathfinder(fileLogger);
            Pathfinder pathfinderConsole = new Pathfinder(consoleLogger);
            Pathfinder pathfinderFridayFile = new Pathfinder(new SecureConsoleLogWritter(weekDay, fileLogger));
            Pathfinder pathfinderFridayConsole = new Pathfinder(new SecureConsoleLogWritter(weekDay, consoleLogger));
            Pathfinder pathfinderFridayFileConsole = new Pathfinder(new SecureConsoleLogWritter(weekDay, fileLogger, consoleLogger));

            pathfinderFile.Find(message);
            pathfinderConsole.Find(message);
            pathfinderFridayFile.Find(message);
            pathfinderFridayConsole.Find(message);
            pathfinderFridayFileConsole.Find(message);

            Console.ReadKey();
        }
    }

    interface ILogger
    {
        void WriteError(string message);
    }

    class ConsoleLogWritter : ILogger
    {
        public void WriteError(string message)
        {            
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogger
    {        
        private string _fileName;

        public FileLogWritter(string fileName, ILogger logger = null)
        {
            _fileName = fileName;            
        }

        public void WriteError(string message)
        {            
            File.AppendAllText(_fileName, $"{message}\n");            
        }
    }

    class SecureConsoleLogWritter : ILogger
    {
        private ILogger[] _loggers;
        private DayOfWeek _weekDay;

        public SecureConsoleLogWritter(DayOfWeek weekDay, params ILogger[] loggers)
        {
            _loggers = loggers;
            _weekDay = weekDay;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == _weekDay)
                foreach (var logger in _loggers)                
                    logger.WriteError(message);
        }
    }

    class Pathfinder
    {
        private ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            _logger = logger;
        }

        public void Find(string message)
        {
            _logger?.WriteError($"{DateTime.Now}: {message}");
        }
    }
}
