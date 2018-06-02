using Microsoft.VisualStudio.TestTools.UnitTesting;
using WCFExchangeSecretarLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFExchangeSecretarLibrary.Tests
{
    [TestClass()]
    public class ExchangeSecretarServiceTests
    {
        [TestMethod()]
        public object MethodFirstTest(params object[] parametrs)
        {
            try
            {
                ExchangeSecretar secretar = new ExchangeSecretar(parametrs[0].ToString(), parametrs[1].ToString(), parametrs[2].ToString(), (IEnumerable<string>)parametrs[3]);
                secretar.Report();
                return true;
            }
            catch (Exception ex)
            {
                return null;
            }
            Assert.Fail();
        }
    }
}