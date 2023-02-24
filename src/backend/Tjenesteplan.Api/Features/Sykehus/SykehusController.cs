using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Sykehus
{
    [Route("api/sykehus")]
    public class SykehusController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly ISykehusRepository _sykehusRepository;

        public SykehusController(
            IUserRepository userRepository,
            ISykehusRepository sykehusRepository
        )
        {
            _userRepository = userRepository;
            _sykehusRepository = sykehusRepository;
        }

        [RoleAuthorization(Role.Admin)]
        [Route("")]
        [HttpPost]
        public IActionResult Add([FromBody] AddSykehusModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _sykehusRepository.AddSykehus(model.Name);
            return Ok(id);
        }

        [RoleAuthorization(Role.Admin, Role.Overlege, Role.Lege)]
        [Route("")]
        [HttpGet]
        public IActionResult Get()
        {
            var username = User.Identity.Name;
            var user = _userRepository.GetUserByUsername(username);
      
            var sykehus = _sykehusRepository.GetSykehus()
                .Where(s => s.Avdelinger.Any(a => CanViewAvdeling(user, a)) || user.Role == Role.Admin)
                .ToList();

            var sykehusModel = sykehus.Select(s => new SykehusModel
            {
                Id = s.Id,
                Name = s.Name,
                Avdelinger = s.Avdelinger
                    .Where(a => CanViewAvdeling(user, a))
                    .Select(a => new AvdelingModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        ListeforerId = a.ListeforerId
                    }).ToList()
            });

            return Ok(sykehusModel);
        }

        private bool CanViewAvdeling(User user, Avdeling avdeling)
        {
            return user.Role == Role.Admin ||
                   avdeling.ListeforerId == user.Id ||
                   user.Avdelinger.Any(id => id == avdeling.Id);
        }

        [RoleAuthorization(Role.Admin)]
        [Route("{id:int}")]
        [HttpPut]
        public IActionResult Update(int id, [FromBody] UpdateSykehusModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _sykehusRepository.UpdateSykehus(id, model.Name);
            return Ok(id);
        }

        [RoleAuthorization(Role.Admin)]
        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _sykehusRepository.DeleteSykehus(id);
            return Ok(id);
        }
    }
}