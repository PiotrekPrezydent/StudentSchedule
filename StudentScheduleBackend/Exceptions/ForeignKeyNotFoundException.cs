namespace StudentScheduleBackend.Exceptions
{
    public class ForeignKeyNotFoundException : Exception
    {
        public ForeignKeyNotFoundException(string message) : base(message) { }

        public ForeignKeyNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
