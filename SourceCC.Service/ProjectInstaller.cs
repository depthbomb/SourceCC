using System.ComponentModel;
using System.ServiceProcess;

namespace SourceCC.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.Installers.Add(GetServiceInstaller());
            this.Installers.Add(GetServiceProcessInstaller());
        }

        private ServiceInstaller GetServiceInstaller()
        {
            serviceInstaller1.ServiceName = "SourceCCService";
            serviceInstaller1.DisplayName = "SourceCC Service";
            serviceInstaller1.Description = "Monitors game directories and automatically cleans cache files.";
            serviceInstaller1.StartType = ServiceStartMode.Automatic;
            return serviceInstaller1;
        }

        private ServiceProcessInstaller GetServiceProcessInstaller()
        {
            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            return serviceProcessInstaller1;
        }
    }
}
