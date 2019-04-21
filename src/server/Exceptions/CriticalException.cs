using System;

namespace Server.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Own exception class extends <see cref="T:System.Exception" /> to implement critical errors
    /// </summary>
    public class CriticalException : Exception
    {
        public CriticalException(string message) : base(message)
        {
        }
    }
}