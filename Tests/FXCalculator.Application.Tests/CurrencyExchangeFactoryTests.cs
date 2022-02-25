using FluentAssertions;
using FXCalculator.Application.CurrencyExchangers;
using FXCalculator.Application.Exceptions;
using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;
using FXCalculator.Data;
using FXCalculator.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace FXCalculator.Application.Tests
{
    public class CurrencyExchangeFactoryTests
    {
        private ICurrencyExchangeFactory _currencyExchangeFactory;
        private IServiceProvider _serviceProvider;

        // test data
        private const string CURRENCY_EXCHANGER_NOT_IMPLEMENTED_EXCEPTION_MESSAGE_NONE = $"Currency exchanger not implement for settlement method 'None'!";

        // arrange
        public CurrencyExchangeFactoryTests()
        {
            //setup DI
            _serviceProvider = new ServiceCollection()
                .AddScoped<ICurrencyExchangeFactory, CurrencyExchangeFactory>()
                .AddScoped<ICurrencyLoader, CurrencyLoader>()
                .AddSingleton<ICurrencyRepository, CurrencyRepository>()
                .AddScoped<DirectExchanger>()
                .AddScoped<CrossExchanger>()
                .AddScoped<OneToOneExchanger>()
                .BuildServiceProvider();

            _currencyExchangeFactory = _serviceProvider.GetService<ICurrencyExchangeFactory>();
        }

        [Fact]
        public void WHEN_SettlementMethodEnum_Is_Not_Setup_In_Factory_THEN_GetCurrencyExchanger_SHOULD_Throw_CurrencyExchangerNotImplementedException()
        {
            // act
            Action act = () => _currencyExchangeFactory.GetCurrencyExchanger(SettlementMethodEnum.None);

            // assert
            act.Should().Throw<CurrencyExchangerNotImplementedException>()
                .WithMessage(CURRENCY_EXCHANGER_NOT_IMPLEMENTED_EXCEPTION_MESSAGE_NONE);
        }

        [Theory]
        [InlineData(SettlementMethodEnum.Cross, nameof(CrossExchanger))]
        [InlineData(SettlementMethodEnum.Direct, nameof(DirectExchanger))]
        [InlineData(SettlementMethodEnum.OneToOne, nameof(OneToOneExchanger))]
        public void WHEN_SettlementMethodEnum_Is_Supplied_THEN_GetCurrencyExchanger_SHOULD_Return_Corresponding_CurrencyExchanger(
         SettlementMethodEnum sellementMethodEnum,
         string exchangerName)
        {
            // act
            var currencyExchanger = _currencyExchangeFactory.GetCurrencyExchanger(sellementMethodEnum);

            // assert
            currencyExchanger.GetType().Name.Should().Be(exchangerName);
        }
    }
}