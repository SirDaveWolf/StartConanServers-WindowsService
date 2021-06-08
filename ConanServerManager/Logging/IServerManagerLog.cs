using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanServerManager.Logging
{
    public interface IServerManagerLog
    {
        void WriteLog(String format, params string[] args);
        void WriteLog(String message);
        void WriteException(String message, Exception ex);
    }
}
