using System;

namespace Tjenesteplan.Api.Features.Tjenesteplaner.Changes
{
    public class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
    }

    public class TjenesteplanUserChangeModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? VakansvaktRequestId { get; set; }
        public int? VaktChangeRequestId { get; set; }
        public User User { get; set; }
    }
}