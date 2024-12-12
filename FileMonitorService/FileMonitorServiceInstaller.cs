using System;
using System.ServiceProcess;
using System.Configuration.Install;

[RunInstaller(true)]
public class FileMonitorServiceInstaller : Installer
{
    public FileMonitorServiceInstaller()
    {
        ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
        serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
        serviceProcessInstaller.Password = null;
        serviceProcessInstaller.Username = null;

        ServiceInstaller serviceInstaller = new ServiceInstaller();
        serviceInstaller.ServiceName = "FileMonitorService";
        serviceInstaller.Description = "File Monitor Service";
        serviceInstaller.DisplayName = "File Monitor Service";
        serviceInstaller.StartType = ServiceStartMode.Automatic;

        Installers.Add(serviceProcessInstaller);
        Installers.Add(serviceInstaller);
    }
}