namespace StudentScheduleBackend.Exceptions
{
    public class ReferentialIntegrityException : Exception
    {
        public ReferentialIntegrityException(string message) : base(message) { }

        public ReferentialIntegrityException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
