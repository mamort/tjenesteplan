using System;
using System.Collections.Generic;
using System.Linq;
using Tjenesteplan.Domain.Actions.MinTjenesteplan;

namespace Tjenesteplan.Domain
{
    public class VaktChangeRequest
    {
        public int Id { get; }
        public int TjenesteplanId { get; }
        public int UserId { get; }
        public DateTime RegisteredDate { get; }
        public DateTime Date { get; }
        public DateTime? ChosenChangeDate { get; }
        public DagsplanEnum Dagsplan { get; }
        public VaktChangeRequestStatus Status { get; }
        public IReadOnlyList<VaktChangeRequestReply> Replies { get; }

        public VaktChangeRequest(
            int id,
            int tjenesteplanId,
            int userId,
            DateTime registeredDate, 
            DateTime date,
            DateTime? chosenChangeDate,
            DagsplanEnum dagsplan,
            VaktChangeRequestStatus status,
            IReadOnlyList<VaktChangeRequestReply> replies
        )
        {
            Id = id;
            TjenesteplanId = tjenesteplanId;
            UserId = userId;
            RegisteredDate = registeredDate;
            Date = date;
            ChosenChangeDate = chosenChangeDate;
            Dagsplan = dagsplan;
            Status = status;
            Replies = replies;
        }

        public IReadOnlyList<DateTime> FindPossibleVaktbytter(
            CurrentTjenesteplan currentTjenesteplan,
            IReadOnlyList<DateTime> suggestedVaktbytteDates)
        {
            return suggestedVaktbytteDates
                .Where(d => currentTjenesteplan.IsDateAvailableForVakt(d, Dagsplan))
                .ToList();
        }
    }
}