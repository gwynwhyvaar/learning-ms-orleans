using GrainInterfaces.SmsSender;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;
namespace Client.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly ILogger<SmsController> logger;
        private readonly IClusterClient clusterClient;
        public SmsController(ILogger<SmsController> logger, IClusterClient clusterClient)
        {
            this.logger = logger;
            this.clusterClient = clusterClient;
        }
        [HttpPost("v1/send/{msisdn}")]
        public async Task<IActionResult> SendSms(string msisdn, [FromBody] string message)
        {
            var grain = this.clusterClient.GetGrain<ISendSms>(0);
            var response = await grain.SendMessage(msisdn, message);
            this.logger.LogInformation($"Sms Sent to {msisdn}");
            return Ok(response);
        }
        [HttpPost("v1/send/oneway/{msisdn}")]
        public IActionResult SendOneWay(string msisdn, [FromBody] string message)
        {
            var grain = this.clusterClient.GetGrain<ISendSms>(0);
            grain.InvokeOneWay(_ => _.SendMessage(msisdn, message));
            this.logger.LogInformation($"Sms Sent to {msisdn}");
            return Accepted("Message Send Ok");
        }
    }
}
