using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tjenesteplan.Api.Features.Tjenesteplaner.AddLege;
using Tjenesteplan.Api.Features.Tjenesteplaner.Changes;
using Tjenesteplan.Api.Features.Tjenesteplaner.ChangeTjenesteplanDateForLege;
using Tjenesteplan.Api.Features.Tjenesteplaner.CreateTjenesteplan;
using Tjenesteplan.Api.Features.Tjenesteplaner.EditTjenesteplan;
using Tjenesteplan.Api.Features.Tjenesteplaner.GetTjenesteplan;
using Tjenesteplan.Api.Features.Tjenesteplaner.GetWeeklyTjenesteplan;
using Tjenesteplan.Api.Features.Tjenesteplaner.RemoveLege;
using Tjenesteplan.Api.Filters;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Repositories;

namespace Tjenesteplan.Api.Features.Tjenesteplaner
{
    [Route("api/tjenesteplaner")]
    public class TjenesteplanController : BaseController
    {
        private readonly ITjenesteplanRepository _tjenesteplanRepository;
        private readonly GetWeeklyTjenesteplanAction _getWeeklyTjenesteplanAction;
        private readonly GetTjenesteplanAction _getTjenesteplanAction;
        private readonly CreateTjenesteplanAction _createTjenesteplanAction;
        private readonly EditTjenesteplanAction _editTjenesteplanAction;
        private readonly AddLegeToTjenesteplanAction _addLegeAction;
        private readonly RemoveLegeFromTjenesteplanAction _removeLegeAction;
        private readonly ChangeTjenesteplanDateForLegeAction _changeTjenesteplanDateForLegeAction;
        private readonly TjenesteplanChangesAction _tjenesteplanChangesAction;

        public TjenesteplanController(
            ITjenesteplanRepository tjenesteplanRepository,
            GetWeeklyTjenesteplanAction getWeeklyTjenesteplanAction,
            GetTjenesteplanAction getTjenesteplanAction,
            CreateTjenesteplanAction createTjenesteplanAction,
            EditTjenesteplanAction editTjenesteplanAction,
            AddLegeToTjenesteplanAction addLegeAction,
            RemoveLegeFromTjenesteplanAction removeLegeAction,
            ChangeTjenesteplanDateForLegeAction changeTjenesteplanDateForLegeAction,
            TjenesteplanChangesAction tjenesteplanChangesAction
        )
        {
            _tjenesteplanRepository = tjenesteplanRepository;
            _getWeeklyTjenesteplanAction = getWeeklyTjenesteplanAction;
            _getTjenesteplanAction = getTjenesteplanAction;
            _createTjenesteplanAction = createTjenesteplanAction;
            _editTjenesteplanAction = editTjenesteplanAction;
            _addLegeAction = addLegeAction;
            _removeLegeAction = removeLegeAction;
            _changeTjenesteplanDateForLegeAction = changeTjenesteplanDateForLegeAction;
            _tjenesteplanChangesAction = tjenesteplanChangesAction;
        }


        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpGet]
        public IActionResult GetTjenesteplaner()
        {
            var tjenesteplaner = _tjenesteplanRepository
                .GetTjenesteplanerCreatedByUser(User.Identity.Name)
                .Select(t => new GetTjenesteplaner.TjenesteplanInfo
                {
                    Id = t.Id,
                    AvdelingId = t.AvdelingId,
                    Name = t.Name
                })
                .ToList();

            return Ok(tjenesteplaner);
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetTjenesteplan(int id)
        {
            try
            {
                var tjenesteplan = _getTjenesteplanAction.Execute(User.Identity.Name, id);
                return Ok(tjenesteplan);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpGet]
        [Route("{id:int}/changes")]
        public IActionResult GetTjenesteplanChanges(int id)
        {
            try
            {
                var changes = _tjenesteplanChangesAction.Execute(User.Identity.Name, id);
                return Ok(changes);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpGet]
        [Route("{id:int}/weekly")]
        public IActionResult GetWeeklyTjenesteplan(int id)
        {
            try
            {
                var tjenesteplan = _getWeeklyTjenesteplanAction.Execute(
                    username: User.Identity.Name,
                    tjenesteplanId: id
                );

                if(tjenesteplan == null)
                {
                    return NotFound();
                }

                return Ok(tjenesteplan);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpPost]
        public async Task<IActionResult> CreateTjenesteplan([FromBody] CreateTjenesteplanModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tjenesteplanId = await _createTjenesteplanAction.ExecuteAsync(
                username: User.Identity.Name,
                model: model
            );

            return Ok(new { id = tjenesteplanId });
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpPut]
        public IActionResult EditTjenesteplan([FromBody] EditTjenesteplanModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _editTjenesteplanAction.Execute(
                username: User.Identity.Name,
                model: model
            );

            return Ok();
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpPost("{id}/leger")]
        public IActionResult AddLegeToTjenesteplan(int id, [FromBody] AddLegeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _addLegeAction.Execute(
                username: User.Identity.Name,
                tjenesteplanId: id,
                model: model
            );

            return Ok();
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpDelete("{id}/leger/{legeId}")]
        public IActionResult RemoveLegeFromTjenesteplan(int id, int legeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _removeLegeAction.Execute(
                username: User.Identity.Name,
                tjenesteplanId: id,
                legeId: legeId
            );

            return Ok();
        }

        [RoleAuthorization(Role.Admin, Role.Overlege)]
        [HttpPut("{id}/leger/{legeId}/datoer/{date}")] 
        public IActionResult ChangeTjenesteplanDateForLege(
            int id, 
            int legeId, 
            DateTime date,
            [FromBody] ChangeTjenesteplanDateForLegeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _changeTjenesteplanDateForLegeAction.Execute(
                username: User.Identity.Name,
                tjenesteplanId: id,
                userId: legeId,
                date: date,
                dagsplan: model.NewDagsplan
            );

            return Ok();
        }
    }
}
