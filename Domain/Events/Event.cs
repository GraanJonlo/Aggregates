namespace Domain.Events
{
    public abstract class Event
    {
        public int Version { get; set; }
    }
}