namespace Tjenesteplan.Api.Features.Tjenesteplaner.GetTjenesteplan
{
    public class LegeInfo
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string Fullname => $"{Firstname} {Lastname}";
        public string Username { get; set; }
    }
}