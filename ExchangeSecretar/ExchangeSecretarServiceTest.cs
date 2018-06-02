using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExchangeSecretar
{
    class ExchangeSecretarServiceTest:ExchangeSecretarService
    {
        public void TestStart(string[] args)
        {
            OnStart(args);
        }

        public void TestStop()
        {
            OnStop();
        }

        public void TestPause()
        {
            OnPause();
        }

        public void TestContinue()
        {
            OnContinue();
        }
    }
}
