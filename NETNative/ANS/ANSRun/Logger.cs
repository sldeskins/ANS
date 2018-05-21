using Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANSRun
{
    public class LoggerFileConsole : ILogger
    {
        public string filename;
        public LoggerFileConsole(string filename)
        {
            this.filename = filename;
            File.Create(filename).Dispose();
        }
        public void Log(string message)
        {
            Console.WriteLine(message);
            File.AppendAllText(filename, message + Environment.NewLine);
        }
    }

}
