using Tjenesteplan.Api.Services.Email;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.EditTjenesteplan
{
    public class NewTjenesteplanAssignedEmail : EmailMessage
    {
        private readonly string _url;

        public override string Title => "Du har blitt tildelt en tjenesteplan";

        public override string Body => CreateEmailBody();

        public NewTjenesteplanAssignedEmail(string email, string url) : base(email)
        {
            _url = url;
        }

        private string CreateEmailBody()
        {
            return "<a href=\"{url}\">Se din tjenesteplan</a>"
                .Replace("{url}", _url);
        }
    }
}