using System;
using System.IO;
using System.Timers;
using System.Diagnostics;
using System.ServiceProcess;
using System.Collections.Generic;

using CaprineNet.INI;
using CaprineNet.UnixTimestamp;

namespace SourceCC.Service
{
    public partial class SourceCC : ServiceBase
    {
        private static string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SourceCC");

        private static string serviceConfigFile = Path.Combine(dataPath, "ServiceConfig.ini");
        //  Paths as in contains the paths we need to check
        private static string pathsConfigFile = Path.Combine(dataPath, "SourceCC.ini");
        private static string logFolder = Path.Combine(dataPath, "Service Logs");

        public SourceCC()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Timer t = new Timer();
            t.Interval = 60000;
            t.AutoReset = true;
            t.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                INI config = new INI(serviceConfigFile);

                int now = new UnixTimestamp().Timestamp;
                int nextRun = int.Parse(config.Read("NextRun", "Operation"));

                if (now >= nextRun)
                {
                    INI pathsConfig = new INI(pathsConfigFile);
                    string[] paths = new string[]
                    {
                        pathsConfig.Read("Tf2", "Folders"),
                        pathsConfig.Read("L4d2", "Folders")
                    };
                    List<string> linesToLog = new List<string>();
                    string logFileName = $"Log-{DateTime.Now.ToString("yyyy-MM-dd")}.log";

                    foreach (string path in paths)
                    {
                        linesToLog.Add(LogLine($"Processing folder {path}"));

                        int filesDeleted = 0;
                        foreach (string file in Directory.EnumerateFiles(path, "*.cache", SearchOption.AllDirectories))
                        {
                            if (File.Exists(file))
                            {
                                File.Delete(file);
                                filesDeleted++;
                                linesToLog.Add(LogLine($"Deleted cache file {file}"));
                            }
                        }

                        if (filesDeleted > 0)
                        {
                            linesToLog.Add(LogLine($"Deleted a total of {filesDeleted} file(s) from {path}"));
                        }
                        else
                        {
                            linesToLog.Add(LogLine($"{path} is already clean!"));
                        }
                    }

                    string runInterval = config.Read("CheckInterval", "Checking");
                    int length;
                    switch (runInterval)
                    {
                        case "hourly":
                            length = 3600;
                            break;
                        case "daily":
                            length = 86400;
                            break;
                        default:
                        case "weekly":
                            length = 604800;
                            break;
                        case "monthly":
                            length = 2592000;
                            break;
                        case "yearly":
                            length = 31556952;
                            break;
                    }

                    int newNextRun = (now + length);

                    config.Write("NextRun", newNextRun.ToString(), "Operation");

                    linesToLog.Add(LogLine($"Setting next schedule run to {newNextRun}"));

                    File.WriteAllLines(Path.Combine(logFolder, logFileName), linesToLog);
                }
            };
            t.Start();
        }

        protected override void OnStop()
        {
        }

        //  Formats a string for use in logging
        private string LogLine(string line)
        {
            string prefix = $"[{DateTime.Now.ToString("HH:mm:ss")}]";
            return $"{prefix} {line}";
        }
    }
}
