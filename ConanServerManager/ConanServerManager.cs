using ConanServerManager.Config;
using ConanServerManager.ConfigLoader;
using ConanServerManager.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConanServerManager
{
    public class ConanServerManager
    {
        private IConanServerConfigLoader _conanServerConfigLoader;
        private IServerManagerLog _logger;
        private ConanServerConfig _serverConfig = null;
        private Dictionary<String, Process> _runningServers;

        internal const int CTRL_C_EVENT = 0;
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        // Delegate type to be used as the Handler Routine for SCCH
        delegate Boolean ConsoleCtrlDelegate(uint CtrlType);

        public List<String> InstanceNames
        {
            get
            {
                if (_serverConfig != null)
                {
                    return _serverConfig.Servers.Select(s => s.Name).ToList();
                }
                else
                {
                    return new List<String>();
                }
            }
        }

        public ConanServerManager(IServerManagerLog logger)
        {
            _logger = logger;

            _runningServers = new Dictionary<String, Process>();
        }

        public ConanServerManager(IConanServerConfigLoader conanServerConfigLoader, IServerManagerLog logger)
            : this(logger)
        {
            _conanServerConfigLoader = conanServerConfigLoader;
            _conanServerConfigLoader.SetLogger(logger);
        }

        public Boolean LoadConfig()
        {
            _serverConfig = _conanServerConfigLoader.Load();
            return _serverConfig != null;
        }

        public void SetConfig(ConanServerConfig conanServerConfig)
        {
            _serverConfig = conanServerConfig;
        }

        public void StartAll()
        {
            if(_serverConfig != null)
            {
                foreach(var server in _serverConfig.Servers)
                {
                    StartByInstanceName(server.Name);
                }
            }
        }

        public void StopAll()
        {
            if (_serverConfig != null)
            {
                foreach (var process in _runningServers.Values)
                {
                    StopServerProcess(process);
                }
            }
        }

        public void StartByInstanceName(String instanceName)
        {
            if (_serverConfig != null)
            {
                var serverStart = new ProcessStartInfo();

                var server = _serverConfig.Servers.FirstOrDefault(s => s.Name == instanceName);

                if (server != null)
                {
                    serverStart.FileName = server.ExePath;
                    serverStart.Arguments = server.Params;

                    if (false == _runningServers.ContainsKey(server.Name))
                    {
                        Process.Start(serverStart);

                        var serverProcesses = Process.GetProcesses().Where(p => p.ProcessName.Contains("ConanSandboxServer-Win64"));
                        foreach (var serverProcess in serverProcesses)
                        {
                            if(false == _runningServers.Any(sp => sp.Value.Id == serverProcess.Id))
                            {
                                _runningServers.Add(server.Name, serverProcess);
                            }
                        }
                    }
                }
                else
                {
                    _logger?.WriteLog($"Could not start server by instance name '{instanceName}'!");
                }
            }
        }

        public void StopServerByInstanceName(String instanceName)
        {
            if (_runningServers.ContainsKey(instanceName))
            {
                if (false == StopServerProcess(_runningServers[instanceName]))
                {
                    _logger?.WriteLog($"Could not stop server by instance name '{instanceName}'!");
                }
            }
        }

        private Boolean StopServerProcess(Process process)
        {
            if (AttachConsole((uint)process.Id))
            {
                SetConsoleCtrlHandler(null, true);
                try
                {
                    if (!GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0))
                        return false;

                    process.WaitForExit();
                }
                finally
                {
                    SetConsoleCtrlHandler(null, false);
                    FreeConsole();
                }
            }
            else
            {
                _logger?.WriteLog($"Could not attach console to process with id '{process.Id}'!");
                var code = Marshal.GetLastWin32Error();
            }

            return true;
        }
    }
}
