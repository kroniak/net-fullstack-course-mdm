using Server.Infrastructure;

namespace Server.Exceptions
{
    /// <summary>
    /// Represent errors for checking and validation
    /// </summary>
    public struct CustomModelError
    {
        public string FieldName { get; set; }

        public string Message { get; set; }

        public string LocalizedMessage { get; set; }

        public TypeCriticalException Type { get; set; }
    }
}