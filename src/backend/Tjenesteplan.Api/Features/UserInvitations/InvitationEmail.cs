using Tjenesteplan.Api.Services.Email;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Api.Features.UserInvitations
{
    public class InvitationEmail : EmailMessage
    {
        private readonly string _url;
        public override string Title => "Du er blitt invitert til Tjenesteplan.no";
        public override string Body => $"Registrer deg <a href=\"{_url}\">her</a>";

        public InvitationEmail(string email, string url) : base(email)
        {
            _url = url;
        }
    }
}