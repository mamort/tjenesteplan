using System;

namespace Tjenesteplan.Domain
{
    public class TjenesteplanChange
    {
        public int Id { get; }
        public int TjenesteplanId { get; }
        public DateTime ChangeDate { get; }
        public DateTime Date { get; }
        public DagsplanEnum Dagsplan { get; }
        public int? VakansvaktRequestId { get; }
        public int? VaktChangeRequestId { get; }

        public TjenesteplanChange(
            int id, 
            int tjenesteplanId, 
            DateTime changeDate, 
            DateTime date, 
            DagsplanEnum dagsplan,
            int? vakansvaktRequestId,
            int? vaktChangeRequestId
        )
        {
            Id = id;
            TjenesteplanId = tjenesteplanId;
            ChangeDate = changeDate;
            Date = date;
            Dagsplan = dagsplan;
            VakansvaktRequestId = vakansvaktRequestId;
            VaktChangeRequestId = vaktChangeRequestId;
        }
    }
}