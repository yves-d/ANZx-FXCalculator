using FXCalculator.Application.CurrencyExchangers;
using FXCalculator.Application.Exceptions;
using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Models;

namespace FXCalculator.Application
{
    public class CurrencyExchangeFactory : ICurrencyExchangeFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private static Dictionary<SettlementMethodEnum, Func<IExchangeCurrency>> _currencyExchangers;

        public CurrencyExchangeFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadCurrencyExchangeServices();
        }

        public IExchangeCurrency GetCurrencyExchanger(SettlementMethodEnum settlementMethodEnum)
        {
            if (_currencyExchangers.ContainsKey(settlementMethodEnum))
                return _currencyExchangers[settlementMethodEnum]();

            throw new CurrencyExchangerNotImplementedException($"Crypto Service not implemented - {settlementMethodEnum}");
        }

        private void LoadCurrencyExchangeServices()
        {
            _currencyExchangers = new Dictionary<SettlementMethodEnum, Func<IExchangeCurrency>>();
            _currencyExchangers.Add(SettlementMethodEnum.Direct, () => (IExchangeCurrency)_serviceProvider.GetService(typeof(DirectExchanger)));
            _currencyExchangers.Add(SettlementMethodEnum.Inverted, () => (IExchangeCurrency)_serviceProvider.GetService(typeof(InvertedExchanger)));
            _currencyExchangers.Add(SettlementMethodEnum.OneToOne, () => (IExchangeCurrency)_serviceProvider.GetService(typeof(OneToOneExchanger)));
            _currencyExchangers.Add(SettlementMethodEnum.Cross, () => (IExchangeCurrency)_serviceProvider.GetService(typeof(CrossExchanger)));
            
        }
    }
}