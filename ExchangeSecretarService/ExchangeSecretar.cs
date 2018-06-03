using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;

namespace WCFExchangeSecretarLibrary
{
    public class ExchangeSecretar:IDisposable
    {
        string exchangeServiceAddress;
        string targetEmailBoxAddress;
        string passwordForThis;
        static List<string> keyWords;
        static private ExchangeService exchangeService;
        static private List<EmailMessage> exchangeSecretarReport;

        static private Collection<ItemId> itemIds { get; set; }

        ~ExchangeSecretar()
        {
            itemIds = null;
            exchangeSecretarReport = null;
        }

        public ExchangeSecretar(string _exchangeServiceAddress, string _targetEmailBoxAddress, string _paswwordForThis, List<string> _keyWords)
        {
            exchangeServiceAddress = _exchangeServiceAddress;
            targetEmailBoxAddress = _targetEmailBoxAddress;
            passwordForThis = _paswwordForThis;
            keyWords = new List<string>();
            foreach (var item in _keyWords)
            {
                keyWords.Add(item);
            }
            exchangeService = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            exchangeService.Credentials = new WebCredentials(targetEmailBoxAddress, passwordForThis);
            exchangeService.AutodiscoverUrl(targetEmailBoxAddress, RedirectionUrlValidationCallback);
            itemIds = new Collection<ItemId>();
            Folder folder = Folder.Bind(exchangeService, WellKnownFolderName.Inbox);
            ItemView view = new ItemView(1);
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false));
            var buffer = folder.FindItems(searchFilter, view).ToList();
            if (buffer == null || buffer.Count == 0)
            {
                return;
            }
            foreach (var item in buffer)
            {
                itemIds.Add(item.Id);
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        private static void GetAllMessageEmail(Collection<ItemId> itemIds)
        {
            if (itemIds == null || itemIds.Count == 0)
            {
                return;
            }
            exchangeSecretarReport = new List<EmailMessage>();
            PropertySet propertySet = new PropertySet(EmailMessageSchema.Subject, EmailMessageSchema.ToRecipients, EmailMessageSchema.IsRead, EmailMessageSchema.Sender, EmailMessageSchema.TextBody, EmailMessageSchema.Body, EmailMessageSchema.DateTimeSent);
            ServiceResponseCollection<GetItemResponse> responses = exchangeService.BindToItems(itemIds, propertySet);
            foreach (GetItemResponse getItemResponse in responses)
            {
                try
                {
                    Item item = getItemResponse.Item;
                    EmailMessage message = (EmailMessage)item;
                    exchangeSecretarReport.Add(message);
                    //Console.WriteLine("Found item {0}.", message.Id.ToString().Substring(144));
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Exception while getting a message: {0}", ex.Message);
                }
            }
            if (responses.OverallResult == ServiceResult.Success)
            {
                //Console.WriteLine("All email messages retrieved successfully.");
                //Console.WriteLine("\r\n");
            }
        }

        static private void GetExchangeSecretarReport()
        {
            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.SetOutputToDefaultAudioDevice();
            GetAllMessageEmail(itemIds);
            if (exchangeSecretarReport == null || exchangeSecretarReport.Count == 0)
            {
                return;
            }
            foreach (EmailMessage item in exchangeSecretarReport)
            {
                foreach (var it in keyWords)
                {
                    if (item.TextBody.Text.Contains(it))
                    {
                        speech.Speak("Уважаемый Алексей Романович вам пришла почта от " + item.Sender.Name + "в письме написано: " + item.TextBody.Text.Remove(item.TextBody.Text.IndexOf("Отправлено", 0)));
                        Collection<ItemId> bufferFoDeleted = new Collection<ItemId>();
                        bufferFoDeleted.Add(item.Id);
                        ServiceResponseCollection<ServiceResponse> response = exchangeService.DeleteItems(bufferFoDeleted, DeleteMode.SoftDelete, null, AffectedTaskOccurrence.AllOccurrences);
                        if (response.OverallResult == ServiceResult.Success)
                        {
                            speech.Speak("Письмо прочтено!");
                            if (exchangeSecretarReport == null || exchangeSecretarReport.Count == 0)
                                return;
                        }
                        else
                        {
                            speech.Speak("Письмо прочтено! Но результат о прочтении записать не удалось");
                        }
                    }
                }
            }
        }

        public void Report()
        {
            GetExchangeSecretarReport();
            itemIds = null;
            exchangeSecretarReport = null;
            exchangeService = null;
        }

        public void Dispose()
        {
            itemIds = null;
            exchangeSecretarReport = null;
            exchangeService = null;
        }
    }
}
