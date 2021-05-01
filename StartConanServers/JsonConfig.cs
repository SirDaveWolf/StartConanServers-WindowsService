using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartConanServers
{
    public class JsonConfig
    {
        private const String Filename = "servers.json";
        private Boolean _doesExist;

        public List<ConanServer> Servers { get; set; }

        public JsonConfig()
        {
            _doesExist = File.Exists(Filename);
        }

        public void Load()
        {
            if (_doesExist)
            {
                var json = File.ReadAllText(Filename);
                dynamic settings = JsonConvert.DeserializeObject(json);

                foreach(var server in settings.Servers)
                {

                }
            }
        }
    }
}
