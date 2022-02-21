﻿using FXCalculator.Common.Models;
using FXCalculator.Data.Exceptions;
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
        private const string FILENAME_CURRENCY_SETTLEMENT_METHOD = "SettlementCurrencies.json";

        private List<CurrencyPairExchangeRate> _currencyPairExchangeRates = new List<CurrencyPairExchangeRate>();
        private List<CurrencyDecimalPrecision> _currencyDecimalPrecisions = new List<CurrencyDecimalPrecision>();
        private List<CurrencySettlementMethod> _currencySettlementMethods = new List<CurrencySettlementMethod>();

        public CurrencyRepository()
        {
            LoadCurrencyPairExchangeRates();
            LoadCurrencyDecimalPrecision();
            LoadCurrencySettlementMethods();
        }

        public CurrencySettlementMethod GetCurrencySettlementMethod(string baseCurrency, string termCurrency)
        {
            var storedMethod = _currencySettlementMethods.SingleOrDefault(method =>
                (method.Base == baseCurrency && method.Term == termCurrency)
                ||
                (method.Base == termCurrency && method.Term == baseCurrency)
            );

            if (storedMethod == null)
                throw new SettlementMethodNotFoundException($"Could not find settlement method for base '{baseCurrency}' and term '{termCurrency}'");

            // swap the base and term around, to represent the complete table
            if (storedMethod.Base == termCurrency)
            {
                return new CurrencySettlementMethod()
                {
                    Base = termCurrency,
                    Term = baseCurrency,
                    SettlementCurrency = storedMethod.SettlementCurrency,
                    SettlementMethod = FlipDirectIndirectConversion(storedMethod.SettlementMethod)
                };
            }
            
            return storedMethod;
        }

        public CurrencyPairExchangeRate GetCurrencyPairExchangeRate(string baseCurrency, string termCurrency)
        {
            return _currencyPairExchangeRates.SingleOrDefault(pair => pair.Base == baseCurrency && pair.Term == termCurrency);
        }

        public CurrencyDecimalPrecision GetCurrencyDecimalPrecision(string currency)
        {
            return _currencyDecimalPrecisions.SingleOrDefault(currencyPrecision => currencyPrecision.Currency == currency);
        }

        private SettlementMethodEnum FlipDirectIndirectConversion(SettlementMethodEnum savedSettlementMethod)
        {
            if (savedSettlementMethod == SettlementMethodEnum.Direct)
                return SettlementMethodEnum.Inverted;
            else if (savedSettlementMethod == SettlementMethodEnum.Inverted)
                return SettlementMethodEnum.Direct;
            else
                return savedSettlementMethod;
        }

        #region Datastore initialisation

        private void LoadCurrencyPairExchangeRates()
        {
            var rawData = LoadRawFileData(CURRENCY_DATA_FOLDER, FILENAME_CURRENCY_PAIR_EXCHANGE_RATES);
                
            _currencyPairExchangeRates.AddRange(JsonConvert.DeserializeObject<List<CurrencyPairExchangeRate>>(rawData));
        }

        private void LoadCurrencyDecimalPrecision()
        {
            var rawData = LoadRawFileData(CURRENCY_DATA_FOLDER, FILENAME_CURRENCY_DECIMAL_PRECISION);

            _currencyDecimalPrecisions.AddRange(JsonConvert.DeserializeObject<List<CurrencyDecimalPrecision>>(rawData));
        }

        private void LoadCurrencySettlementMethods()
        {
            var rawData = LoadRawFileData(CURRENCY_DATA_FOLDER, FILENAME_CURRENCY_SETTLEMENT_METHOD);

            _currencySettlementMethods.AddRange(JsonConvert.DeserializeObject<List<CurrencySettlementMethod>>(rawData));
        }

        private string LoadRawFileData(string path, string fileName)
        {
            var rawData = DataFileReader.ReadFromFile(path, fileName);
            if (rawData == null)
                throw new DatastoreReadFailureException($"Unable to read from {path}/{fileName}");

            return rawData;
        }

        #endregion
    }
}