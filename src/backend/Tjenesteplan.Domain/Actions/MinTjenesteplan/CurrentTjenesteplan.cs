using System;
using System.Collections.Generic;
using System.Linq;

namespace Tjenesteplan.Domain.Actions.MinTjenesteplan
{
    public class CurrentTjenesteplan
    {
        public IReadOnlyList<TjenesteDag> Dates { get; }

        public CurrentTjenesteplan(List<TjenesteDag> dates)
        {
            Dates = dates;
        }

        public bool IsDateAvailableForVakt(DateTime date, DagsplanEnum dagsplan)
        {
            var tjenesteplanDay = Dates.FirstOrDefault(d => d.Date.Date == date.Date);

            /* Foreløpig gjør vi det enkelt å sier at dag ikke er tilgjengelig for vakt hvis man allerede har en vakt den
            * dagen uavhengig av hvilken type vakt det er (dagvakt, nattevakt, døgnvakt), men i de fleste andre tilfeller
            * */
            var availableForVaktDagsplaner = new HashSet<DagsplanEnum>
            {
                DagsplanEnum.None,
                DagsplanEnum.Avspasering,
                DagsplanEnum.Ferie,
                DagsplanEnum.FordypningForskning,
                DagsplanEnum.FriEtterVakt,
                DagsplanEnum.KursKonferanseMøte,
                DagsplanEnum.Permisjon,
                DagsplanEnum.UkesAvspasering,
                DagsplanEnum.Universitet
            };

            return tjenesteplanDay == null ||
                   availableForVaktDagsplaner.Contains(tjenesteplanDay.Dagsplan);

        }
    }
}