using FXCalculator.Application.Interfaces;
using FXCalculator.Data;
using FXCalculator.Data.Interfaces;
using Xunit;

namespace FXCalculator.Application.Tests
{
    public class FXCalculatorServiceTests
    {
        IFXCalculatorService _fxCalculatorService;

        ICurrencyRepository _currencyRepository;

        public FXCalculatorServiceTests()
        {
            _currencyRepository = new CurrencyRepository();
            _fxCalculatorService = new FXCalculatorService(_currencyRepository);
        }

        [Fact]
        public void WHEN_Currency_Is_Not_Found_THEN_CalculateExchangeAmount_SHOULD_Return_CURRENCYNOTFOUND()
        {
            // arrange

            // act


            // assert
        }

        [Fact]
        public void WHEN_Currency_Pair_Is_Not_Found_THEN_CalculateExchangeAmount_SHOULD_Return_PAIRNOTFOUND()
        {
            // arrange

            // act

            // assert
        }

        [Fact]
        public void WHEN_Currency_Amount_Is_Zero_THEN_CalculateExchangeAmount_SHOULD_Return_CANNOTCONVERTZEROAMOUNT()
        {
            // arrange

            // act

            // assert
        }

        [Fact]
        public void WHEN_Currency_Pair_AND_Rate_Exists_THEN_CalculateExchangeAmount_SHOULD_Return_Converted_Amount()
        {
            // arrange

            // act

            // assert
        }
    }
}