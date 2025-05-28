using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Exchange.WebServices.Data;

namespace Gefco.CipQuai.Web
{
    public static partial class Instances
    {
        private static ExchangeService _exchangeService;
        public static ExchangeService ExchangeService
        {
            get
            {
                if (_exchangeService == null)
                    _exchangeService = GetBinding();
                return _exchangeService;
            }
            set
            {
                if (_exchangeService == value) return;
                _exchangeService = value;
            }
        }

        static Instances()
        {
            if (_exchangeService == null)
                InitExchangeService();
        }

        private static void InitExchangeService()
        {
            Console.WriteLine("Initializing connection");
            _exchangeService = GetBinding();
        }
        private static ExchangeService GetBinding()
        {
            // Create the binding.
            var service = new ExchangeService
            {
                TraceEnabled = true,
                TraceFlags = TraceFlags.All,
            };
            Console.WriteLine("Exchange Service : Autoconfiguring connection");
            service.Credentials = new WebCredentials(Properties.Settings.Default.EWSMail, Properties.Settings.Default.EWSPass);
            try
            {
                //service.Url = new Uri("https://outlook.com/EWS/Exchange.asmx");
                service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            }
            catch (Exception ex)
            {
                SimpleLogger.GetOne().Error("EngineService.GetBinding : An error occurred.", ex);
            }

            return service;
        }

        public static void SendEmail(string subject, string body, params EmailAddress[] recipients)
        {
            var email = new EmailMessage(Instances.ExchangeService);

            foreach (var recipient in recipients)
            {
                email.BccRecipients.Add(recipient);
            }
            email.Subject = "Cip Quai - " + subject;
            email.From = new EmailAddress("no-reply", "no-reply@sensor6ty.com");
            email.Body = new MessageBody(BodyType.HTML, body);
            email.SendAndSaveCopy();
        }
    }
}