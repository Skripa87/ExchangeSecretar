using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ExchangeSecretar
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            //ExchangeSecretarServiceTest test = new ExchangeSecretarServiceTest();
            //test.TestStart(new string[0]);
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ExchangeSecretarService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
