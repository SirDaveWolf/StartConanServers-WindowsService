using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanServerManager.Logging
{
    public class DefaultServerManagerLog : IServerManagerLog
    {
        private readonly String LogDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ConanExilesServerManager");
        private readonly String LogFile = "log.txt";

        public void WriteException(String message, Exception ex)
        {
            WriteLog($"{message} Exception: {ex.Message}");
            WriteLog(ex.StackTrace);
        }

        public void WriteLog(String format, params String[] args)
        {
            WriteLog(String.Format(format, args));
        }

        public void WriteLog(String message)
        {
            Directory.CreateDirectory(LogDirectory);
            File.AppendAllText(Path.Combine(LogDirectory, LogFile), $"{DateTime.Now} - {message}\n");
        }
    }
}
