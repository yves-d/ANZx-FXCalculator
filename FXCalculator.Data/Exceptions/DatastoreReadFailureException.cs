namespace FXCalculator.Data.Exceptions
{
    public class DatastoreReadFailureException : Exception
    {
        public DatastoreReadFailureException()
        {
        }

        public DatastoreReadFailureException(string message)
            : base(message)
        {
        }

        public DatastoreReadFailureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}