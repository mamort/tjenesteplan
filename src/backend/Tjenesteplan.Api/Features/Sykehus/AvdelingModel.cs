namespace Tjenesteplan.Api.Features.Sykehus
{
    public class AvdelingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ListeforerId { get; set; }
    }
}