namespace ServiceReference1
{
    using System.Runtime.Serialization;
    using System;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IExchangeSecretarService")]
    public interface IExchangeSecretarService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IExchangeSecretarService/MethodFirst", ReplyAction = "http://tempuri.org/IExchangeSecretarService/MethodFirstResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        object[] ExcecuteMasterFunction(object[] parametrs);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IExchangeSecretarService/MethodTwo", ReplyAction = "http://tempuri.org/IExchangeSecretarService/MethodTwoResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        object[] GetActualServiceParametrs(object[] parametrs);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IExchangeSecretarService/MethodThree", ReplyAction = "http://tempuri.org/IExchangeSecretarService/MethodThreeResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        object[] ChangeServiceParametrs(object[] parametrs);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IExchangeSecretarService/MethodForth", ReplyAction = "http://tempuri.org/IExchangeSecretarService/MethodForthResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        object[] MethodForth(object[] parametrs);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IExchangeSecretarServiceChannel : IExchangeSecretarService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class ExchangeSecretarServiceClient : System.ServiceModel.ClientBase<IExchangeSecretarService>, IExchangeSecretarService
    {

        public ExchangeSecretarServiceClient()
        {
        }

        public ExchangeSecretarServiceClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public ExchangeSecretarServiceClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ExchangeSecretarServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ExchangeSecretarServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public object[] ExcecuteMasterFunction(object[] parametrs)
        {
            return base.Channel.ExcecuteMasterFunction(parametrs);
        }

        public object[] GetActualServiceParametrs(object[] parametrs)
        {
            return base.Channel.GetActualServiceParametrs(parametrs);
        }

        public object[] ChangeServiceParametrs(object[] parametrs)
        {
            return base.Channel.ChangeServiceParametrs(parametrs);
        }

        public object[] MethodForth(object[] parametrs)
        {
            return base.Channel.MethodForth(parametrs);
        }
    }
}