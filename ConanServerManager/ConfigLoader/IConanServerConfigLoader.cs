using ConanServerManager.Config;
using ConanServerManager.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanServerManager.ConfigLoader
{
    public interface IConanServerConfigLoader
    {
        void SetLogger(IServerManagerLog serverManagerLog);
        ConanServerConfig Load();
    }
}
