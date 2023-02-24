using System;
using Tjenesteplan.Domain;

namespace Tjenesteplan.Api.Features.VakansvaktRequests.AddVakansvaktRequest
{
    public class AddVakansvaktRequestModel
    {
        public int TjenesteplanId { get; set; }
        public DateTime Date { get; set; }

        public DagsplanEnum Dagsplan { get; set; }
        public string Reason { get; set; }
    }
}