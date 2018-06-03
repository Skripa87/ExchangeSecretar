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
        static ServiceHost service_host = null;
        static ExchangeSecretarServiceClient client = null;
        static System.Speech.Synthesis.SpeechSynthesizer speech;
        static EventLog eventLog;

        public ExchangeSecretarService()
        {
            InitializeComponent();
            speech = new System.Speech.Synthesis.SpeechSynthesizer();
            speech.SetOutputToDefaultAudioDevice();
            eventLog = new EventLog();
            this.AutoLog = false;
            if(!EventLog.SourceExists("SecretarSource"))
            {
                EventLog.CreateEventSource("SecretarSource", "SecretarLog");
            }
            eventLog.Source = "SecretarSource";
            eventLog.Log = "SecretarLog";
        }

        protected override void OnStart(string[] args)
        {
            try
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
                try
                {
                    service_host.Open();
                    eventLog.WriteEntry("Была создана точка подключения к сервису WCF \n");
                }
                catch (Exception ex)
                {
                    eventLog.WriteEntry("Не удалось создать точку подключения к сервису WCF \n" + ex.InnerException.ToString());
                }

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 60000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
                timer.Start();
            }
            catch(Exception ex)
            {
                eventLog.WriteEntry("Ошибка в процессе создания точки подключения к WCF сервису \n");
                eventLog.WriteEntry("Подробности: "+ ex.ToString());
            }

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
                    eventLog.WriteEntry("Поток исполнения сновной задачи службы запущен \n");
                }
                catch (Exception ex)
                {
                    eventLog.WriteEntry("Ошибка запуска потока исполнения основной задачи службы \n");
                    eventLog.WriteEntry("Подробности " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry("Ошибка при попытке создания клиента WCF сервиса \n");
                eventLog.WriteEntry("Подробности " + ex.ToString());
            }
        }

        private void Create_New_Client()
        {
            if (client == null)
            {
                try
                {
                    Try_To_Create_New_Client();
                }
                catch (Exception ex)
                {
                    speech.Speak(ex.ToString());
                    speech.Speak(ex.InnerException.ToString());
                    client = null;
                    eventLog.WriteEntry("Ошибка при попытке создания клиента WCF сервиса \n");
                    eventLog.WriteEntry("Подробности " + ex.ToString());
                }
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
                string uri = "net.tcp://192.168.1.32:9002/MyService";
                EndpointAddress endpoint = new EndpointAddress(new Uri(uri));
                client = new ExchangeSecretarServiceClient(binding, endpoint);
                client.ClientCredentials.Windows.ClientCredential.Domain = "";
                client.ClientCredentials.Windows.ClientCredential.UserName = "skrip";
                client.ClientCredentials.Windows.ClientCredential.Password = "PLUSHKAgasha777";                
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry("Ошибка при попытке создания клиента WCF сервиса.-Возможные причины проблема доступа!\n");
                eventLog.WriteEntry("Подробности " + ex.ToString());
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
