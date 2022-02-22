using Autofac;
using FXCalculator.Application.Interfaces;
using FXCalculator.Data;
using FXCalculator.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXCalculator.Application.Tests
{
    public  class FXCalculatorServiceTestHarness
    {
        IFXCalculatorService _fxCalculatorService;

        ICurrencyRepository _currencyRepository;
        ICurrencyExchangeFactory _currencyExchangeFactory;
        IServiceProvider _serviceProvider;


        public FXCalculatorServiceTestHarness()
        {

        }

        private void ConfigureServices()
        {
            //setup our DI
            _serviceProvider = new ServiceCollection()
                .AddTransient<IFXCalculatorService, FXCalculatorService>()
                .AddSingleton<ICurrencyRepository, CurrencyRepository>()
                .AddTransient<ICurrencyExchangeFactory, CurrencyExchangeFactory>()
                .BuildServiceProvider();
        }
    }
}
