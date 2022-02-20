using FXCalculator.Common.Models;
using FXCalculator.Data.Helpers;
using FXCalculator.Data.Interfaces;
using Newtonsoft.Json;

namespace FXCalculator.Data
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private const string CURRENCY_DATA_FOLDER = "CurrencyData";
        private const string FILENAME_CURRENCY_PAIR_EXCHANGE_RATES = "CurrencyPairExchangeRates.json";
        private const string FILENAME_CURRENCY_DECIMAL_PRECISION = "CurrencyDecimalPrecision.json";

        private List<CurrencyPairExchangeRate> _currencyPairExchangeRates;
        private List<CurrencyDecimalPrecision> _currencyDecimalPrecision;

        public CurrencyRepository()
        {
            LoadCurrencyPairExchangeRates();
        }

        private void LoadCurrencyPairExchangeRates()
        {
            var rawData = DataFileReader.ReadFromFile(CURRENCY_DATA_FOLDER, FILENAME_CURRENCY_PAIR_EXCHANGE_RATES);

            if (rawData == null)
                throw new Exception();
                
            _currencyPairExchangeRates = JsonConvert.DeserializeObject<List<CurrencyPairExchangeRate>>(rawData);
        }

        private void LoadCurrencyDecimalPrecision()
        {
            var rawData = DataFileReader.ReadFromFile(CURRENCY_DATA_FOLDER, FILENAME_CURRENCY_DECIMAL_PRECISION);

            if (rawData == null)
                throw new Exception();

            _currencyDecimalPrecision = JsonConvert.DeserializeObject<List<CurrencyDecimalPrecision>>(rawData);
        }
    }
}