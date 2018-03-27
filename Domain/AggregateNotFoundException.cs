using System;

namespace Domain
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException() : base()
        {
        }

        protected AggregateNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        public AggregateNotFoundException(string message) : base(message)
        {
        }

        public AggregateNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}