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
        private ConanServerManager.ConanServerManager _serverManager;
        private ConanServerManager.Logging.IServerManagerLog _logger;

        public StartConanServersService()
        {
            _logger = new ConanServerManager.Logging.DefaultServerManagerLog();
            InitializeComponent();
            _serverManager = new ConanServerManager.ConanServerManager(new ConanServerManager.ConfigLoader.JsonConanServerConfigLoader(), _logger);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                
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
