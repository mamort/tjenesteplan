using System;
using Tjenesteplan.Api.Services.Email;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Api.Features.Users.ResetPassword
{
    public class ResetPasswordEmail : EmailMessage
    {
        private readonly string _url;
        public override string Title => "Tilbakestill passord";
        public override string Body => CreateEmailBody();

        public ResetPasswordEmail(
            string email, 
            string url)
            : base(email)
        {
            _url = url;
        }


        private string CreateEmailBody()
        {
            return "Hvis du ønsker å sette nytt passord kan du klikke på følgende lenke: <a href=\"{url}\">Tilbakestill passord</a>"
                .Replace("{url}", _url);
        }

    }
}