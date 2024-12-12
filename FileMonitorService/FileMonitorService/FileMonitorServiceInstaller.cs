using System;
using System.ServiceProcess;
using System.ComponentModel;
using System.Configuration.Install;

[RunInstaller(true)]
public class FileMonitorServiceInstaller : Installer
{
    private ServiceInstaller serviceInstaller;
    private ServiceProcessInstaller processInstaller;

    public FileMonitorServiceInstaller()
    {
        // Create a new ServiceInstaller instance
        serviceInstaller = new ServiceInstaller();
        serviceInstaller.ServiceName = "FileMonitorService";
        serviceInstaller.DisplayName = "File Monitor Service";
        serviceInstaller.Description = "A service that monitors files.";
        serviceInstaller.StartType = ServiceStartMode.Automatic;

        // Create a new ServiceProcessInstaller instance
        processInstaller = new ServiceProcessInstaller();
        processInstaller.Account = ServiceAccount.LocalSystem;

        // Add the installers to the Installers collection
        Installers.Add(serviceInstaller);
        Installers.Add(processInstaller);
    }
}