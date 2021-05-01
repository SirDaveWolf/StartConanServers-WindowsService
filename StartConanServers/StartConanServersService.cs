using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace StartConanServers
{
    public partial class StartConanServersService : ServiceBase
    {
        private JsonConfig serverConfig; 

        public StartConanServersService()
        {
            InitializeComponent();
            serverConfig = new JsonConfig();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                serverConfig.Load();
                foreach(var server in serverConfig.Servers)
                {
                    var startInfo = new ProcessStartInfo();

                    startInfo.FileName = server.ExePath;
                    startInfo.Arguments = server.Params;

                    // TODO Configure Database Backup
                    // TODO Server Log

                    Process.Start(startInfo);
                }
            }
            catch(Exception ex)
            {
                // TODO: Servicelog
            }
        }

        protected override void OnStop()
        {
            // TODO: Stop all server instances
        }
    }
}
