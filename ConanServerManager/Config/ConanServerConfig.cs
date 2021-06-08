using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanServerManager.Config
{
    public class ConanServerConfig
    {
        public ConanServerConfig()
        {
            Servers = new List<ConanServer>();
        }

        public String BackupDirectory { get; set; }
        public List<ConanServer> Servers { get; set; }
    }
}
