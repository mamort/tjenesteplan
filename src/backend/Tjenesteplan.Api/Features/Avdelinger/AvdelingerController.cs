using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Avdelinger
{
    [Route("api")]
    public class AvdelingerController : BaseController
    {
        private readonly ISykehusRepository _sykehusRepository;
        private readonly IAvdelingRepository _avdelingRepository;
        private readonly IUserRepository _userRepository;

        public AvdelingerController(
            ISykehusRepository sykehusRepository,
            IAvdelingRepository avdelingRepository,
            IUserRepository userRepository
        )
        {
            _sykehusRepository = sykehusRepository;
            _avdelingRepository = avdelingRepository;
            _userRepository = userRepository;
        }

        [RoleAuthorization(Role.Admin)]
        [Route("sykehus/{sykehusId:int}/avdelinger")]
        [HttpPost]
        public IActionResult Add(int sykehusId, [FromBody] AddAvdelingModel model)
        {
            var sykehus = _sykehusRepository.GetSykehus(sykehusId);
            if (sykehus == null)
            {
                return BadRequest($"Could not find sykehus with id {sykehusId}");
            }

            var id = _avdelingRepository.AddAvdeling(sykehusId, model.Name);
            return Ok(id);
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [Route("sykehus/{sykehusId:int}/avdelinger")]
        [HttpGet]
        public IActionResult Get(int sykehusId)
        {
            var avdeling = _avdelingRepository.GetAvdelinger(sykehusId);
            return Ok(avdeling);
        }

        [RoleAuthorization(Role.Admin)]
        [Route("avdelinger/{id:int}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody] UpdateAvdelingModel model)
        {
            var currentAvdeling = _avdelingRepository.GetAvdeling(id);

            if (currentAvdeling == null)
            {
                return BadRequest($"Avdeling with id {id} does not exist.");
            }

            if (model.ListeforerId.HasValue)
            {
                var avdelinger = _avdelingRepository.GetAllAvdelinger();
                var oldListeforerId = currentAvdeling?.ListeforerId;

                var user = _userRepository.GetUserById(model.ListeforerId.Value);

                if (user == null)
                {
                    return BadRequest($"Could not find listefører user with id {model.ListeforerId.Value}");
                }

                _userRepository.UpdateUserRole(user.Id, Role.Overlege);

                // If old listeforer no longer is a listeforer for any avdeling => degrade role to 'Lege'
                if (currentAvdeling.ListeforerId != model.ListeforerId && oldListeforerId.HasValue)
                {
                    if (!avdelinger.Any(a => a.ListeforerId == oldListeforerId && a.Id != id))
                    {
                        _userRepository.UpdateUserRole(oldListeforerId.Value, Role.Lege);
                    }
                }
            }

            _avdelingRepository.UpdateAvdeling(id, model.Name, model.ListeforerId);

            return Ok(id);
        }

        [RoleAuthorization(Role.Admin)]
        [Route("avdelinger/{id:int}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _avdelingRepository.DeleteAvdeling(id);
            return Ok(id);
        }
    }
}