using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using ServiceReference1;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeSecretar
{
    public partial class ExchangeSecretarService : ServiceBase
    {
        private ServiceHost service_host = null;
        ExchangeSecretarServiceClient client = null;
        //public static object[] Parametrs { get; set; }
        //public static List<string> Param4 { get; set; }
        static System.Speech.Synthesis.SpeechSynthesizer speech;
        public ExchangeSecretarService()
        {
            InitializeComponent();
            speech = new System.Speech.Synthesis.SpeechSynthesizer();
            speech.SetOutputToDefaultAudioDevice();
            //Parametrs = new object[4];
        }

        protected override void OnStart(string[] args)
        {
            if (service_host != null) service_host.Close();

            string address_HTTP = "http://localhost:9001/MyService";
            string address_TCP = "net.tcp://localhost:9002/MyService";

            Uri[] address_base = { new Uri(address_HTTP), new Uri(address_TCP) };
            service_host = new ServiceHost(typeof(WCFExchangeSecretarLibrary.ExchangeSecretarService), address_base);

            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            service_host.Description.Behaviors.Add(behavior);

            BasicHttpBinding binding_http = new BasicHttpBinding();
            service_host.AddServiceEndpoint(typeof(WCFExchangeSecretarLibrary.IExchangeSecretarService), binding_http, address_HTTP);
            service_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            NetTcpBinding binding_tcp = new NetTcpBinding();
            binding_tcp.Security.Mode = SecurityMode.Transport;
            binding_tcp.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding_tcp.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            binding_tcp.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            service_host.AddServiceEndpoint(typeof(WCFExchangeSecretarLibrary.IExchangeSecretarService), binding_tcp, address_TCP);
            service_host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

            service_host.Open();

            //Parametrs[0] = ""; Parametrs[1] = "a.skrypin@krus-zapad.ru";
            //Parametrs[2] = "srvxH4#"; Parametrs[3] = "привет!;пока;идиотик";

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

        }
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                Create_New_Client();
                try
                {
                    Task task = new Task(() => { client.MethodFirst(new object[0]); });
                    task.Start();
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        protected override void OnContinue()
        {
            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = 60000; // 60 seconds  
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            //timer.Start();
        }

        private void Create_New_Client()
        {
            if (client == null)
                try { Try_To_Create_New_Client(); }
                catch (Exception ex)
                {
                    speech.Speak(ex.ToString());
                    speech.Speak(ex.InnerException.ToString());
                    client = null;
                }
            else
            {
            }
        }
        private void Try_To_Create_New_Client()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.Transport);
                binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                string uri = "net.tcp://192.168.1.32:9002/MyService";//"net.tcp://172.17.127.129:9002/MyService";
                EndpointAddress endpoint = new EndpointAddress(new Uri(uri));
                client = new ExchangeSecretarServiceClient(binding, endpoint);
                client.ClientCredentials.Windows.ClientCredential.Domain = "";
                client.ClientCredentials.Windows.ClientCredential.UserName = "skrip";
                client.ClientCredentials.Windows.ClientCredential.Password = /*"";//*/"PLUSHKAgasha777";
                //var test = client.MethodFirst(new object[0]);
                //if (test == null)
                //{
                //    throw new Exception("Проверка соединения не удалась");
                //}
            }
            catch (Exception ex)
            {
                speech.Speak("Нет соединения с сервисом по причине! "+ex.ToString());
                client = null;
            }
        }
        protected override void OnStop()
        {
            if (service_host != null)
            {
                service_host.Close();
                service_host = null;
            }
        }
    }
}
