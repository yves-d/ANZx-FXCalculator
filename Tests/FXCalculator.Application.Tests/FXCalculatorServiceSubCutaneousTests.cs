using FluentAssertions;
using FXCalculator.Application.CurrencyExchangers;
using FXCalculator.Application.Interfaces;
using FXCalculator.Data;
using FXCalculator.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace FXCalculator.Application.Tests
{
    public class FXCalculatorServiceSubCutaneousTests
    {
        IFXCalculatorService _fxCalculatorService;

        ICurrencyRepository _currencyRepository;
        ICurrencyExchangeFactory _currencyExchangeFactory;
        IServiceProvider _serviceProvider;

        public FXCalculatorServiceSubCutaneousTests()
        {
            //_currencyRepository = new CurrencyRepository();
            //_currencyExchangeFactory = new CurrencyExchangeFactory();
            //_fxCalculatorService = new FXCalculatorService(_currencyRepository, _currencyExchangeFactory);

            //setup our DI
            _serviceProvider = new ServiceCollection()
                .AddTransient<IFXCalculatorService, FXCalculatorService>()
                .AddSingleton<ICurrencyRepository, CurrencyRepository>()
                .AddTransient<ICurrencyExchangeFactory, CurrencyExchangeFactory>()
                //.AddTransient<ServiceResolver>(serviceProvider => key =>
                //{
                //    switch (key)
                //    {
                //        case "A":
                //            return serviceProvider.GetService<ServiceA>();
                //        case "B":
                //            return serviceProvider.GetService<ServiceB>();
                //        case "C":
                //            return serviceProvider.GetService<ServiceC>();
                //        default:
                //            throw new KeyNotFoundException(); // or maybe return null, up to you
                //    }
                //});
                .AddScoped<DirectExchanger>()
                .AddScoped<InvertedExchanger>()
                .AddScoped<CrossExchanger>()
                .AddScoped<OneToOneExchanger>()
                .BuildServiceProvider();

            _fxCalculatorService = _serviceProvider.GetService<IFXCalculatorService>();
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

        
        [Theory]
        [InlineData("AUD", "USD", 100.00, 83.71)]
        [InlineData("CAD", "USD", 100.00, 87.11)]
        [InlineData("USD", "CNY", 100.00, 617.15)] // cross table data is incorrect for usd/cny (listed as inverted) - should be direct feed
        [InlineData("EUR", "USD", 100.00, 123.15)]
        [InlineData("GBP", "USD", 100.00, 156.83)]
        [InlineData("NZD", "USD", 100.00, 77.50)]
        [InlineData("USD", "JPY", 100.00, 11995.00)]
        [InlineData("EUR", "CZK", 100.00, 2760.28)]
        [InlineData("EUR", "DKK", 100.00, 744.05)]
        [InlineData("EUR", "NOK", 100.00, 866.51)]
        public void WHEN_Currency_Pair_Has_Direct_Feed_THEN_CalculateExchangeAmount_SHOULD_Return_Converted_Amount(
            string baseCurrency,
            string termCurrency,
            decimal amountToConvert,
            decimal expectedAmount)
        {
            // arrange & act
            var convertedAmount = _fxCalculatorService.CalculateExchangeAmount(baseCurrency, termCurrency, amountToConvert);

            // assert
            convertedAmount.ExchangedAmount.Should().Be(expectedAmount);
        }

        [Theory]
        [InlineData("USD", "AUD", 100.00, 119.46)]
        [InlineData("USD", "CAD", 100.00, 114.80)]
        [InlineData("CNY", "USD", 100.00, 16.20)] // cross table data is incorrect for usd/cny (listed as inverted) - should be direct feed
        [InlineData("USD", "EUR", 100.00, 81.20)]
        [InlineData("USD", "GBP", 100.00, 63.76)]
        [InlineData("USD", "NZD", 100.00, 129.03)]
        [InlineData("JPY", "USD", 100.00, 1.00)]
        [InlineData("CZK", "EUR", 100.00, 3.62)]
        [InlineData("DKK", "EUR", 100.00, 13.44)]
        [InlineData("NOK", "EUR", 100.00, 11.54)]
        public void WHEN_Currency_Pair_Has_Inverted_Feed_THEN_CalculateExchangeAmount_SHOULD_Return_Converted_Amount(
            string baseCurrency,
            string termCurrency,
            decimal amountToConvert,
            decimal expectedAmount)
        {
            // arrange & act
            var convertedAmount = _fxCalculatorService.CalculateExchangeAmount(baseCurrency, termCurrency, amountToConvert);

            // assert
            convertedAmount.ExchangedAmount.Should().Be(expectedAmount);
        }

        [Theory]
        [InlineData("AUD", "AUD", 100.00, 100.00)]
        [InlineData("CAD", "CAD", 100.00, 100.00)]
        [InlineData("CNY", "CNY", 100.00, 100.00)]
        [InlineData("CZK", "CZK", 100.00, 100.00)]
        [InlineData("DKK", "DKK", 100.00, 100.00)]
        [InlineData("EUR", "EUR", 100.00, 100.00)]
        [InlineData("GBP", "GBP", 100.00, 100.00)]
        [InlineData("JPY", "JPY", 100.00, 100.00)]
        [InlineData("NOK", "NOK", 100.00, 100.00)]
        [InlineData("NZD", "NZD", 100.00, 100.00)]
        [InlineData("USD", "USD", 100.00, 100.00)]
        [InlineData("AUD", "AUD", 100.01, 100.01)]
        [InlineData("CAD", "CAD", 100.01, 100.01)]
        [InlineData("CNY", "CNY", 100.01, 100.01)]
        [InlineData("CZK", "CZK", 100.01, 100.01)]
        [InlineData("DKK", "DKK", 100.01, 100.01)]
        [InlineData("EUR", "EUR", 100.01, 100.01)]
        [InlineData("GBP", "GBP", 100.01, 100.01)]
        [InlineData("JPY", "JPY", 100.01, 100.00)]
        [InlineData("NOK", "NOK", 100.01, 100.01)]
        [InlineData("NZD", "NZD", 100.01, 100.01)]
        [InlineData("USD", "USD", 100.01, 100.01)]
        [InlineData("AUD", "AUD", 100.90, 100.90)]
        [InlineData("CAD", "CAD", 100.90, 100.90)]
        [InlineData("CNY", "CNY", 100.90, 100.90)]
        [InlineData("CZK", "CZK", 100.90, 100.90)]
        [InlineData("DKK", "DKK", 100.90, 100.90)]
        [InlineData("EUR", "EUR", 100.90, 100.90)]
        [InlineData("GBP", "GBP", 100.90, 100.90)]
        [InlineData("JPY", "JPY", 100.90, 101.00)]
        [InlineData("NOK", "NOK", 100.90, 100.90)]
        [InlineData("NZD", "NZD", 100.90, 100.90)]
        [InlineData("USD", "USD", 100.90, 100.90)]
        public void WHEN_Currency_Pair_Is_OneToOne_THEN_CalculateExchangeAmount_SHOULD_Return_Same_Amount_Rounded_To_Currency_Decimal_Precision(
            string baseCurrency,
            string termCurrency,
            decimal amountToConvert,
            decimal expectedAmount)
        {
            // arrange & act
            var convertedAmount = _fxCalculatorService.CalculateExchangeAmount(baseCurrency, termCurrency, amountToConvert);

            // assert
            convertedAmount.ExchangedAmount.Should().Be(expectedAmount);
        }

        [Theory]
        [InlineData("AUD", "CAD", 100.00, 96.10)]
        [InlineData("AUD", "CNY", 100.00, 516.62)]
        [InlineData("AUD", "CZK", 100.00, 2310.63)]
        //[InlineData("AUD", "DKK", 100.00, 81.20)]
        //[InlineData("AUD", "EUR", 100.00, 63.76)]
        //[InlineData("AUD", "GPB", 100.00, 129.03)]
        //[InlineData("AUD", "JPY", 100.00, 129.03)]
        //[InlineData("AUD", "NOK", 100.00, 129.03)]
        //[InlineData("AUD", "NZD", 100.00, 129.03)]
        public void WHEN_Currency_Pair_Uses_A_Cross_Currency_THEN_CalculateExchangeAmount_SHOULD_Return_Converted_Amount(
            string baseCurrency,
            string termCurrency,
            decimal amountToConvert,
            decimal expectedAmount)
        {
            // arrange & act
            var convertedAmount = _fxCalculatorService.CalculateExchangeAmount(baseCurrency, termCurrency, amountToConvert);

            // assert
            convertedAmount.ExchangedAmount.Should().Be(expectedAmount);
        }
    }
}