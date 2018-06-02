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
        object[] MethodFirst(object[] parametrs);
        [OperationContract]
        object[] MethodTwo(object[] parametrs);
        [OperationContract]
        object[] MethodThree(object[] parametrs);
        [OperationContract]
        object[] MethodForth(object[] parametrs);
    }
}
