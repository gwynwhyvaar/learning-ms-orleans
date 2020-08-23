using GrainInterfaces.SmsSender;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
namespace Grains.SmsSender
{
    public class SendSms : Orleans.Grain, ISendSms
    {
        private readonly ILogger logger;
        public SendSms(ILogger<SendSms> logger)
        {
            this.logger = logger;
        }
        public Task<string> SendMessage(string msisdn, string smsMessage)
        {
            logger.LogInformation($"\n Sms received: message = '{smsMessage}' to = <{msisdn}>");
            // todo: add an SMS implementation here.
            return Task.FromResult($"\n Sms Sender: Accepted for Delivery");
        }
    }
}
