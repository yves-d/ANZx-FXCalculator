namespace FXCalculator.Application.Exceptions
{
    public class CrossViaMethodNotFoundException : Exception
    {
        public CrossViaMethodNotFoundException()
        {
        }

        public CrossViaMethodNotFoundException(string message)
            : base(message)
        {
        }

        public CrossViaMethodNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
