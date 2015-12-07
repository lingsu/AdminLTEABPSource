using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Net.Mail;
using Microsoft.AspNet.Identity;

namespace LyuAdmin.Users
{
    public class EmailService: IIdentityMessageService, ITransientDependency
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendAsync(IdentityMessage message)
        {
            await  _emailSender.SendAsync(message.Destination, message.Subject, message.Body);
        }
    }
}