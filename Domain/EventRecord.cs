using System;

namespace Domain
{
    public readonly struct EventRecord
    {
        public readonly Guid Uuid;
        public readonly int Version;
        public readonly string EventData;

        public EventRecord(Guid uuid, int version, string eventData)
        {
            Uuid = uuid;
            Version = version;
            EventData = eventData;
        }
    }
}
