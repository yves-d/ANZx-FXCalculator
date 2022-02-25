Console.WriteLine("Welcome to the ANZx FXCalculator!" + Environment.NewLine);
Console.WriteLine("---------------------------------" + Environment.NewLine);
Console.WriteLine(Environment.NewLine);
Console.WriteLine("Type 'exit' at any time, to end the program" + Environment.NewLine);
Console.WriteLine(Environment.NewLine);
Console.WriteLine("To convert a currency, enter the base currency, followed by the amount, the word 'in', and finally the term currency you wish to conver to. For example..." + Environment.NewLine);
Console.WriteLine("AUD 100.00 in USD" + Environment.NewLine);

bool exit = false;
var command = Console.ReadLine();
while (!exit)
{
    if (command.ToLower() == "exit")
    {
        exit = true;
        continue;
    }
    try
    {
        CommandReader commandReader = new CommandReader(command, currentArena, currentRobot);
        commandReader.ExecuteCommand();

        command = Console.ReadLine();
        currentArena = commandReader.CommandReaderArena;
        currentRobot = commandReader.CommandReaderRobot;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        command = Console.ReadLine();
    }
}