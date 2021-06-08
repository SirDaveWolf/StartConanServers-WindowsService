using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StartConanServerTest
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();

        static void Main(string[] args)
        {
            var logger = new ConanServerManager.Logging.DefaultServerManagerLog();
            var manager = new ConanServerManager.ConanServerManager(new ConanServerManager.ConfigLoader.JsonConanServerConfigLoader(), 
                                                                    logger);

            if(manager.LoadConfig())
            {
                var instanceName = manager.InstanceNames.FirstOrDefault();
                if(instanceName != null)
                {
                    manager.StartByInstanceName(instanceName);
                    Console.WriteLine($"Server {instanceName} started! Waiting 10 seconds...");

                    Thread.Sleep(10000);



                    Console.WriteLine($"Going to stop {instanceName}. Press ENTER to continue...");
                    Console.ReadKey();

                    FreeConsole();
                    manager.StopServerByInstanceName(instanceName);
                }
                else
                {
                    Console.WriteLine("No instance name! Check log!");
                    Console.ReadKey();
                }
            }
        }
    }
}
