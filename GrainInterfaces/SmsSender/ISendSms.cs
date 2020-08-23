using System.Threading.Tasks;
namespace GrainInterfaces.SmsSender
{
    public interface ISendSms: Orleans.IGrainWithIntegerKey
    {
        Task<string> SendMessage(string msisdn, string smsMessage);
    }
}
