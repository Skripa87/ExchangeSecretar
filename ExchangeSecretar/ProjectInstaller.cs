using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace WCFExchangeSecretarLibrary
{
    [RunInstaller(true)]
    public partial class ExchangeSecretarServiceInstaller : Installer
    {
        public ExchangeSecretarServiceInstaller()
        {
            //InitializeComponent();
            serviceProcessInstaller1 = new ServiceProcessInstaller();
            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            serviceInstaller1 = new ServiceInstaller();
            serviceInstaller1.ServiceName = "ExchangeSecretar";
            serviceInstaller1.DisplayName = "ExchangeSecretar";
            serviceInstaller1.Description = "WCF Service Hosted by Windows NT Service";
            serviceInstaller1.StartType = ServiceStartMode.Automatic;
            Installers.Add(serviceProcessInstaller1);
            Installers.Add(serviceInstaller1);
        }
    }
}
