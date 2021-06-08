using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConanServerManager.Config;
using ConanServerManager.Logging;
using Newtonsoft.Json;

namespace ConanServerManager.ConfigLoader
{
    public class JsonConanServerConfigLoader : IConanServerConfigLoader
    {
        private const String Filename = "servers.json";
        private IServerManagerLog _logger;

        public void SetLogger(IServerManagerLog serverManagerLog)
        {
            _logger = serverManagerLog;
        }

        public ConanServerConfig Load()
        {
            var config = new ConanServerConfig();

            try
            {
                if (File.Exists(Filename))
                {
                    var json = File.ReadAllText(Filename);
                    dynamic settings = JsonConvert.DeserializeObject(json);

                    config.BackupDirectory = settings.BackupDirectory;

                    foreach (var server in settings.Servers)
                    {
                        var serverSetting = new ConanServer();

                        serverSetting.Name = server.Name;
                        serverSetting.Params = server.Params;
                        serverSetting.ExePath = server.ExePath;

                        config.Servers.Add(serverSetting);
                    }
                }
                else
                {
                    _logger?.WriteLog($"Unable to load config file '{Filename}'! File does not exist!");
                    return null;
                }
            }
            catch(Exception ex)
            {
                _logger?.WriteException($"Unable to load json file '{Filename}'!", ex);
                return null;
            }

            return config;
        }
    }
}
