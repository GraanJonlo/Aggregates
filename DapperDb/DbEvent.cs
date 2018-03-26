using System;

namespace DapperDb
{
    public class DbEvent
    {
        public int Id { get; set; }
        public string StreamId { get; set; }
        public Guid Uuid { get; set; }
        public int Version { get; set; }
        public string EventData { get; set; }
    }

    public class DbVersion
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public int Version { get; set; }
    }
}