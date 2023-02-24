namespace Tjenesteplan.Events
{
    public class EventEnvelope
    {
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}