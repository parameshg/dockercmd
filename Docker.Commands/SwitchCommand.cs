using GoCommando;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Management.Automation.Runspaces;
using System.Net;

namespace Docker.Commands
{
    [Command("switch", group: "docker")]
    public class SwitchCommand : ICommand
    {
        [Parameter("os", shortName: "o", optional: false)]
        public string OS { get; set; }

        [Parameter("url", shortName: "u", optional: true, defaultValue: "http://localhost:2375")]
        public string Url { get; set; }

        public void Run()
        {
            dynamic info = JsonConvert.DeserializeObject<dynamic>(new WebClient().DownloadString($"{Url}/info"));

            var os = ((string)info.OSType).ToLower();

            if ((OS == "windows" && os == "linux") || (OS == "linux" && os == "windows"))
            {
                var cmd = new Process()
                {
                    StartInfo = new ProcessStartInfo(@"C:\Program Files\Docker\Docker\DockerCli.exe", "-SwitchDaemon")
                    {
                        Verb = "runas",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };

                cmd.Start();

                cmd.WaitForExit();
            }
        }
    }
}