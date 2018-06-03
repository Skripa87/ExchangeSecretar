using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WCFExchangeSecretarLibrary
{
    [ServiceContract]
    public interface IExchangeSecretarService
    {
        [OperationContract]
        object[] ExcecuteMasterFunction(object[] parametrs);
        [OperationContract]
        object[] GetActualServiceParametrs(object[] parametrs);
        [OperationContract]
        object[] ChangeServiceParametrs(object[] parametrs);
        [OperationContract]
        object[] MethodForth(object[] parametrs);
    }
}
