using System;
using System.Net;
using Server.Infrastructure;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Server.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Own exception class extends <see cref="T:System.Exception" /> to implement critical errors
    /// </summary>
    public class CriticalException : Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public TypeCriticalException TypeCriticalException { get; protected set; }

        public CriticalException(string message) : base(message)
        {
        }

        public CriticalException(string message, TypeCriticalException type = TypeCriticalException.USER) :
            base(message)
        {
            TypeCriticalException = type;
        }

        /// <inheritdoc />
        public CriticalException(string message, TypeCriticalException type = TypeCriticalException.USER,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
            TypeCriticalException = type;
        }
    }
}