using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WCFExchangeSecretarLibrary
{
    public class ExchangeSecretarService : IExchangeSecretarService
    {
        static object[] _parametrs;
        public static object[] Parametrs { get { return _parametrs; } }
        private static EventLog eventLog;

        public ExchangeSecretarService()
        {
            eventLog = new EventLog();
            if (!EventLog.SourceExists("WCFServiceSource"))
            {
                EventLog.CreateEventSource("WCFServiceSource", "WCFServiceLog");
            }
            eventLog.Source = "WCFServiceSource";
            eventLog.Log = "WCFServiceLog";
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

        public object[] ExcecuteMasterFunction(object[] parametrs)
        {
            List<string> parametr4 = new List<string>();
            foreach(var p in _parametrs[3].ToString().Split(';'))
            {
                parametr4.Add(p);
            }
            try
            {
                ExchangeSecretar secretar = new ExchangeSecretar(_parametrs[0].ToString(), _parametrs[1].ToString(), _parametrs[2].ToString(), parametr4);
                secretar.Report();
                eventLog.WriteEntry("Объект экземпляр онсовного исполняющего класса создан успешно \n");
                return new object[]{ true};
            }
            catch(Exception ex)
            {
                eventLog.WriteEntry("Ошибка в процессе создания объекта - экземпляра основного исполняющего класса \n");
                eventLog.WriteEntry("Подробности: " + ex.ToString());
                return null;
            }
        }

        public object[] MethodForth(object[] parametrs)
        {
            return new object[4] { _parametrs[0], _parametrs[1], _parametrs[2], _parametrs[3] };
        }

        public object[] ChangeServiceParametrs(object[] parametrs)
        {
            return new object[1] { new object() };
        }

        public object[] GetActualServiceParametrs(object[] parametrs)
        {
            return Parametrs;
        }
    }
}
