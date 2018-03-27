using System;

namespace Domain
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException() : base()
        {
        }

        protected ConcurrencyException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        public ConcurrencyException(string message) : base(message)
        {
        }

        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}