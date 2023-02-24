using System;
using Microsoft.Extensions.Options;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Api.Services.Email;
using Tjenesteplan.Domain.Api;
using Tjenesteplan.Domain.Api.Email;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Users.ResetPassword
{
    public class ResetPasswordAction
    {
        private readonly CommonOptions _commonOptions;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ResetPasswordAction(
            IOptions<CommonOptions> commonOptions,
            IUserRepository userRepository, 
            IEmailService emailService)
        {
            _commonOptions = commonOptions.Value;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public void Execute(ResetPasswordModel model)
        {
            var token = Guid.NewGuid().ToString();
            _userRepository.SetResetPasswordToken(model.Email, token);

            var url = $"{_commonOptions.BaseUrl}/tilbakestill-passord/{token}";
            _emailService.Send(new ResetPasswordEmail(model.Email, url));
        }
    }
}