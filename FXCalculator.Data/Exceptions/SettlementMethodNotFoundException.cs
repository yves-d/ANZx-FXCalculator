namespace FXCalculator.Data.Exceptions
{
    public class SettlementMethodNotFoundException : Exception
    {
        public SettlementMethodNotFoundException()
        {
        }

        public SettlementMethodNotFoundException(string message)
            : base(message)
        {
        }

        public SettlementMethodNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
