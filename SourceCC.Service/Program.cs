using System.ServiceProcess;

namespace SourceCC.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SourceCC()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
