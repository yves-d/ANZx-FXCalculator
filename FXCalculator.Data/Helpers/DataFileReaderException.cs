namespace FXCalculator.Data.Helpers
{
    public class DataFileReaderException : Exception
    {
        public DataFileReaderException()
        {
        }

        public DataFileReaderException(string message)
            : base(message)
        {
        }
    }
}