using System.Threading.Tasks;

namespace Tjenesteplan.Domain.Api.Email
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
        Task SendAsync(EmailMessage emailMessage);
    }
}