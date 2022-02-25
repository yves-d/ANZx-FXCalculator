namespace FXCalculator.Application.Exceptions
{
    public class CurrencySettlementMethodNotFoundException : Exception
    {
        public CurrencySettlementMethodNotFoundException()
        {
        }

        public CurrencySettlementMethodNotFoundException(string message)
            : base(message)
        {
        }

        public CurrencySettlementMethodNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}