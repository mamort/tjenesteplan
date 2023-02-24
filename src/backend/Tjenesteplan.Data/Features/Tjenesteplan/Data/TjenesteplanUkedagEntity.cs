using System;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Data.Features.Tjenesteplan.Data
{
    public class TjenesteplanUkedagEntity
    {
        public int Id { get; set; }
        public int TjenesteplanId { get; set; }
        public int TjenesteplanUkeId { get; set; }   

        public TjenesteplanUkeEntity TjenesteplanUke { get; set; }
        public DateTime Date { get; set; }
        public DagsplanEnum Dagsplan { get; set; }
    }
}