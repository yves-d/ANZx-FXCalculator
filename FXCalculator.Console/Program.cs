using FXCalculator.Application;
using FXCalculator.Application.CurrencyExchangers;
using FXCalculator.Application.Interfaces;
using FXCalculator.Common.Logger;
using FXCalculator.Console;
using FXCalculator.Console.Interfaces;
using FXCalculator.Data;
using FXCalculator.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

//setup our DI
IServiceProvider _serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))
    .AddScoped<IFXCalculatorConsoleService, FXCalculatorConsoleService>()
    .AddScoped<IFXCalculatorService, FXCalculatorService>()
    .AddScoped<ICurrencyLoader, CurrencyLoader>()
    .AddSingleton<ICurrencyRepository, CurrencyRepository>()
    .AddScoped<ICurrencyExchangeFactory, CurrencyExchangeFactory>()
    .AddScoped<DirectExchanger>()
    .AddScoped<CrossExchanger>()
    .AddScoped<OneToOneExchanger>()
    .BuildServiceProvider();

IFXCalculatorConsoleService _fxCalculatorConsoleService = _serviceProvider.GetService<IFXCalculatorConsoleService>();

Console.WriteLine("Welcome to the ANZx FXCalculator!" + Environment.NewLine);
Console.WriteLine("---------------------------------" + Environment.NewLine);
Console.WriteLine("Type 'exit' at any time, to end the program" + Environment.NewLine);
Console.WriteLine(Environment.NewLine);
Console.WriteLine("To convert a currency, enter the base currency, followed by the amount, then the word 'in', and finally the term currency you wish to convert to." + Environment.NewLine);
Console.WriteLine("For example... AUD 100.00 in USD" + Environment.NewLine);

bool exit = false;
while (!exit)
{
    Console.WriteLine("Please Enter your FX request: ");

    var command = Console.ReadLine();
    if (!string.IsNullOrEmpty(command))
    {
        if (command.ToLower() == "exit")
        {
            exit = true;
            continue;
        }
        try
        {
            var result = _fxCalculatorConsoleService.GetCurrencyConversionResponse(command);
            Console.WriteLine(result + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}