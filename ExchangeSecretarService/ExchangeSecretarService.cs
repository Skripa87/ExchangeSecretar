using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFExchangeSecretarLibrary
{
    public class ExchangeSecretarService : IExchangeSecretarService
    {
        static object[] _parametrs;

        ExchangeSecretarService()
        {
            _parametrs = new object[4];
            SetDefaultServiceParametrs();
        }
        private void SetDefaultServiceParametrs()
        {
            _parametrs[0] = "";
            _parametrs[1] = "a.skrypin@krus-zapad.ru";
            _parametrs[2] = "srvxH4#";
            _parametrs[3] = "привет;ключ";
        }

        public object[] MethodFirst(object[] parametrs)
        {
            List<string> parametr4 = new List<string>();
            foreach (var p in _parametrs[3].ToString().Split(';'))
            {
                parametr4.Add(p);
            }
            try
            {
                ExchangeSecretar secretar = new ExchangeSecretar(_parametrs[0].ToString(), _parametrs[1].ToString(), _parametrs[2].ToString(), parametr4);
                secretar.Report();
                return new object[] { 1 };
            }
            catch (Exception ex)
            {
                return new object[] { 2 };
            }
        }

        public object[] MethodForth(object[] parametrs)
        {
            return new object[1] { new object() };
        }

        public object[] MethodThree(object[] parametrs)
        {
            return new object[1] { new object() };
        }

        public object[] MethodTwo(object[] parametrs)
        {
            return new object[1] { new object() };
        }
    }
}
