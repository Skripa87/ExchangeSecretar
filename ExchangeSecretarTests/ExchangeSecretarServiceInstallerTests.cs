using Microsoft.VisualStudio.TestTools.UnitTesting;
using WCFExchangeSecretarLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFExchangeSecretarLibrary.Tests
{
    [TestClass()]
    public class ExchangeSecretarServiceInstallerTests
    {
        [TestMethod()]
        public void ExchangeSecretarServiceInstallerTest()
        {
            serviceProcessInstaller1 = new ServiceProcessInstaller();
            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            serviceInstaller1 = new ServiceInstaller();
            serviceInstaller1.ServiceName = "ExchangeSecretar";
            serviceInstaller1.DisplayName = "ExchangeSecretar";
            serviceInstaller1.Description = "WCF Service Hosted by Windows NT Service";
            serviceInstaller1.StartType = ServiceStartMode.Automatic;
            Installers.Add(serviceProcessInstaller1);
            Installers.Add(serviceInstaller1);
            Assert.Fail();
        }
    }
}