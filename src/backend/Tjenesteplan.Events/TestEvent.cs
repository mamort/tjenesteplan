using System;

namespace Tjenesteplan.Events
{
    [Serializable]
    public class TestEvent : ITjenesteplanEvent
    {
        public string Name { get; set; }
    }
}
