# ANZx-FXCalculator
ANZx - Programming Exercise – FX Calculator

## Language
This coding exercise was implemented in C#, running in Dot Net Core 6.

## Running
Provided below are two options for running the code, depending on your requirements and local machine setup.

The easiest way to build, run, and crucially debug this solution, is to install Microsoft VisualStudio (not VisualStudio Code), and run it in the IDE.

It may be necessary to install the .Net Core 6 SDK on your machine (if not already), to get the solution running in Visual Studio.

To open the solution, select the ``ANZx-FXCalculator.sln`` file in VisualStudio.

Alternatively, if debugging the code isn't a concern and you just wish to run the solution, there is a command-line option (only requiring the installation of the .Net Core SDK).

### Running Options

#### Visual Studio
Within Visual Studio, the solution can be debugged as a .NetCore app, by running or debugging the unit tests from the Test Explorer (this can be found in the 'View' menu).

At a minimum, you would need to download the Community version of Visual Studio, from the [Visual Studio website](https://visualstudio.microsoft.com/downloads/).

#### Command Line

##### Running the solution from the command line
The command line option is probably the most straight-forward method of running the solution.

If you haven't already, install the [.Net Core 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) from Microsoft.

Once installed, using your favourite console / terminal application, navigate to the root folder of where the solution has been copied to (the folder where the ``ANZx-FXCalculator.sln`` file is located), and type or copy/paste: 

``dotnet run --project "./FXCalculator.Console/FXCalculator.Console.csproj"``, and hit Enter.

Exact syntax may very, depending on your machine (Windows / Mac / Linux). The above syntax is for a Windows machine.

This should build and run the solution.

##### Running the unit tests from the command line
Similarly, you can also run the unit tests from the command line.

From the same folder location as above...

For the FXCalculator.Application tests, type or copy/paste: 

``dotnet test "./Tests/FXCalculator.Application.Tests/FXCalculator.Application.Tests.csproj"`` 

For the FXCalculator.Console tests, type or copy/paste: 

``dotnet test "./Tests/FXCalculator.Console.Tests/FXCalculator.Console.Tests.csproj"`` 

## Analysis and development approach

### Analysis - Initial Observations, Thoughts, and Questions:
1. How to store the table of rates?
- Initial thoughts are to represent it in a json file, and load it up at application startup.
- This should allow for future extensibility of reading from a datastore or external API.

2. Looks like the types of currency settlement available on the cross table are distinct 'methods' of settling between currencies.
- each would require their own distinct way of settling an exchange.
- Potential there for a factory class.

3. Are all the settlement currency mappings in the provided table complete? Are there any gaps between currencies? Are they mapped correctly?
- Without actually eyeballing each one, I guess we'll find out at the unit test stage.

4. Do all the currency pairs in the table have corresponding pairs in the rates list?
- Looks like there is an intended error output which would cover this scenario, which we can build unit tests for.

5. How to store the table of settlement currency mappings?
- Initial thoughts are to go with a JSON file, and load at application start-up.
- At first glance, each pair and their settlement currency could be represented by a distinct json object in the file. 
- This data mapping could potentially be checked first, before any other lookup is required.

6. How to store the decimal place information?
- This could certainly go into a json file.
- Consider - how likely is this to change? Perhaps its something that can be hardcoded.
- Seeing as we're already likely to go with a JSON file store, may as well keep to that pattern for this as well. Think of it as a stand-in for a database.

7. Do we need to load up the JSON data store objects for unit tests, or do we mock/stub that?
- Seeing as it would effectively be in-memory, may as well load it up for unit tests as well.

8. In terms of validation and exception handling... 
    
    a. What happens when incorrect input is entered?
    - As the application (business logic) layer itself doesn't need to worry about this, I would have this occur closer to the console (presentation) layer. As it will have to parse the typed user input, it will need to perform basic hygiene with regards to formatting and type casting.
    - Once the data is cleaned, the presentation layer can pass what was entered down to the business layer. It can then validate currencies, amounts, etc. Further, the methods of our service are strongly typed, so providing invalid input (such as a string or characters instead of a decimal) is not possible by the time the data hits the business layer.

    b. Is there anything exceptional we need to consider, with regards to what the user has entered?
    - Amounts: For zero amounts, no harm done - just return the calculated amount (zero).
    - Currencies: we have instructions on how to handle the case of not being able to find a currency pair.

    c. What if a currency entered doesn't exist?
    - Potentially we could treat it in a similar manner to not being able to find a currency pair. We can say that currency was not found.

    d. Do we need to validate transactions with a negative amount?
    - Can't exchange negative amounts, so yes.

    e. Should we return an exception from the Application layer to the console?
    -  My answer would vary, depending on the use case. I think if the Application layer can't do what it was asked to do, then yes, let's throw an exception, and allow the Conole to catch it and present the appropriate information, depending on the exception thrown.

### Order Of development
1. Started the README.md to commence jotting down these thoughts.
2. Created the 'FXCalculator.Application' project, to represent the application (business logic) layer. This will sit behind the console app. It could equally sit behind an API server - intention here is to go with a ports-and-connector / hexagonal architecture pattern.
3. Added the interface for interacting with the calculation service in the application layer - IFXCalculatorService.
4. Created the 'FXCalculationResult' class and 'FXCalculationOutcomeEnum' to represent the result of a transaction.
5. Defined the methods for IFXCalculatorService, all returning 'FXCalculationResult'.
6. Created a service class to inherit IFXCalculatorService, and implemented the methods with default responses that throw 'NotImplementedException' (temporary).
7. Created the test project 'FXCalculator.Application.Tests'.
8. Created 'FXCalculatorServiceTests' to test FXCalculatorService.
9. Began populating tests.
10. Created the 'FXCalculator.Data' to act as the data layer for the solution.
11. Created a repository interface 'ICurrencyRepository' to represent interaction with the data persistence layer, for the loading of currency pairs & rates, settlement currencies, and currency precision.
12. Created repository class 'CurrencyRepository' to inherit 'ICurrencyRepository', and implemented the methods with default responses that throw 'NotImplementedException' (temproary).
13. Added sample data in json files to the 'FXCalculator.Data' project.
14. Start populating CurrencyRepository.
15. Decided to store the data as json files to be read at startup.
16. Took the suggestion of using the symmetry of the data in the table to cut down on data entry. Have to account for this in code when loading reverse pairs.
17. Created 'CurrencyLoader' class to sit between the CurrencyRepository and the FXCalculatorService. Intention here is to handle any manipulation of pairs before serving it up to the 'FXCurrencyService'.
18. Built a factory to produce currency exchangers for direct, indirect, one-to-one, and cross settlement methods.
19. First run of the happy path unit tests for direct pairs picked up that the cross table entry for CNY/USD (direct) - USD/CNY (inverted) aren't playing nice. 
20. Googled the current price of US dollars to Chinese Yuan, and noticed it's in the region of ~6.33, so the direct feed data is in the range for a USD/CNY pair, therefore the table guidance is wrong for this pair. CNY/USD should be inverted, and USD/CNY should be direct.
    - Decision to be made: do we fix the table, or account for 'errors' in the table in code?
    - Guidance in the instructions said to use the table provided. Assumption is this is a deliberate error.
    - Will go with keeping the table as is, and make the direct/inverted rate lookup more robust.
21. We can handle the issue of incorrect table guidance by doing away with the distinction between direct and inverted all together, as far as the service, factory, and currency exchangers are concerned. In effect, the distinction for direct/inverted as seen in the table can be rendered meaningless - they are both considered a direct look-up. Instead, we can work out the correct rate when we ask for the exchange pair from the 'CurrencyLoader'.
22. The CurrencyLoader can work out if a pair it has been provided is inverted or not (when viewed against the currency pair rate table), and switch up the rate (1/rate) on the fly.
23. Tidied up the unit tests.
24. Created 'FXCalculator.Console' project to contain the console app to interact through.
25. Created 'FXCalculatorConsoleService' to handle validation of user input, and act as the main entry point to the 'FXCalculatorService'.
26. Went with a RegEx approach to validate user input - seemed like it would simplify things.
27. Added the different running options.
28. Filled out the rest of the README.md

## ANZx FXCalculator Solution
The solution is divided into six projects - four concerned with the application itself, and the remaining two are testing projects.

The projects are: 

1. FXCalculator.Application

2. FXCalculator.Common

3. FXCalculator.Data

4. FXCalculator.Console

5. FXCalculator.Application.Tests

6. FXCalculator.Console.Tests

### FXCalculator.Application
The FXCalculator.Application project exists as the core domain of the 'FXCalculator' business logic, and houses the main service, models, interface, and custom exceptions, needed to process input from the console project class.

Ordinarily, if this solution were running in a server environment (for example), the FXCalculator.Application project would be referenced by an API (or other presentation) layer in a 'ports and connector' type pattern. In fact, using this pattern, an API layer could indeed be placed alongside the consolse project and utilise the exact same entry point into the application, and consume the exact same return objects. This is indeed the point of the 'ports and connector' (sometimes referred to as a hexagonal pattern) - keeping business logic segregated from the presentation layer.

The main entry-point into the FXCalculator.Application functionality is through the FXCalculatorService. 

The FXCalculatorService itself loads two dependencies, the 'ICurrencyLoader' - an interface to represent the interaction with the data layer (more on the ICurrencyLoader below), and the 'ICurrencyExchangeFactory', which creates classes for dealing with the various types of conversion method specified in the cross table.

It's straightforward in design, and contains no logic/decision trees. It merely connects the various elements of the application together.

In order of operation:

1. Load the settlement method for the supplied currency pair, from the cross table.

2. Load the appropriate currency exchanger via the 'ICurrencyExchangeFactory', based on the settlement method.

3. Load the 'ExchangeInstrument' based off the settlement method - that is to say, load all the necessary data to calculation the rate and determine the decimal precision.

4. Finally, exchange the amount, using the 'ExchangeIntrument'.

#### CurrencyLoader
The CurrencyLoader is the intermediary between the data layer and the application layer. As the data in the cross table only has half of the data provided (using the symmetrical approach hinted at in the instructions), the CurrencyLoader completes the opposite pairs, if the data is not present in the data layer.

Similarly, it also inverts the rates for direct pairs, if the currencies are swapped around and are the opposite to the listed pairs.

#### CurrencyExchangeFactory
The CurrencyExchangeFactory outputs the corresponding currency exchanger for the given settlement method. Within each currency exchanger is the logic to extract the correct rate, and produce an exchange instrument that will perform the conversion of the amount.

### FXCalculator.Common
As the name suggests, the FXCalculator.Common project is used as a location for storing models and functionality common across the projects.

### FXCalculator.Data
FXCalculator.Data is the data layer. It houses the class that interact with the data store, the 'CurrencyRepository', as well as the actual data to represent the rates, decimal precision, and cross table, all of which I have chosen to represent as json objects, for simplicity. Also included are various helpers to load the data from the stored json files.

Ordinarily in a real-world application, the data layer (CurrencyRepository, in this case) would have been represented by .Net Core's Entity Framework database context (or similar). Therefore when approaching unit tests, 'CurrencyRepository' would potentially have had to have been substituted with a mock implementation via an external mocking library. This would have meant that every interaction with the data layer would have required a mocked response, which would have blown out the size of test project.

Alternatively, it's conceivable that the data may have been consumed from an external API. Given there are likely performance questions involved in both a data store and an external API, it may have been desirable to cache the data from these sources for a small amount of time, to limit the amount of service calls necessary.

### FXCalculator.Console
FXCalculator.Console is the main entry point into the app. It launches the 'FXCalculatorConsoleService', which performs some simple user input validation through the use of a RegEx, and then proceeds to extract the base currency, amount, and term currency from the user's input, which it then passes directly to the implmentation of 'IFXCalculatorService', mentioned previously.

If an exception is thrown by the 'IFXCalculatorService' implementation, it determines if the message is appropriate to show to the user (such as a pair not being available or valid, versus not being able to load the right currency exchanger via the factory class), and log any message that shouldn't be shown to the user.

As this is a simple exercise, in this instance I chose to directly return the exception message to the user, as it seemed like overkill to do otherwise and build an entirely fresh message. If this were an API or UI layer (for example), I would have sought to build any messages for the user's eyes here at the presentation layer, and logged any exception messages (if necessary).

### FXCalculator.Application.Tests
FXCalculator.Application.Tests is where we can find the unit tests for the 'FXCalculator.Application' project.

Within, there are six unit test files, five of which focus on the various classes in the business layer, and implement mocked/substituted dependencies and stubbed responses from those dependencies. These are targetted unit tests that test the specific functionality of each class.

The sixth test file is the 'FXCalculatorServiceSubcutaneousTests'. I call them subcutaneous because they effectively test the entire application from the top down, without using mocked dependencies. This was one of the benefits of using json files for the data in the data project - it enabled the testing of the entire application with actual data, without having to mock any of that data. Through this method, I was able to determine that the cross table entry for CNY/USD was swapped around the wrong way, relative to the pair given on the rates table.

Approaching it this way - broader multiple interaction tests, AND targetted unit tests, helps answer the question "what is our application doing at the level of the smallest unit, and does it all come together in a cohesive approach?"

### FXCalculator.Console.Tests
Again, as the name suggests, FXCalculator.Console.Tests tests the validation handling of user input, as well as the way the console handles the situation when the 'FXCalculatorService' throws an exception, to ensure failures are handled gracefully so that the user can continue to interact with the program.

## Assumptions, Trade-offs & Additional Notes
1. For the purposes of keeping this exercise simpler, authentication and authorization were assumed to not be required, therefore each request is assumed to be valid for the consumer.

2. Minimal logging was implemented, as there was no specific requirement.

3. As the method of data persistence wasn't specified, I went with loading json files, as it was straightforward and easy to test with. Additionally, it's not too far removed from an API call, so I felt it gave the flexibility of going in either direction of an external API call, or even something like a NoSQL db (MongoDB for example). It also streamlined the testing approach, as we could use actual data, as opposed to a mocked data store and stubbed responses from that data store.

4. I didn't devote any time to defensive coding of the data store, i.e. wrapping data store logic in try/catch blocks, as it would have been overkill for the data-store approach that was implemented. In short, the defensive code would have been defending against a scenario that would never happen. I would have had to mock that un-happy path in unit tests, and in effect it would not be a real test of the code implemented, and therefore not provide any additional value.

5. *If* an exception were to occur at the data store layer, it would be caught at the console layer, logged, and a generic error would be shown to the user.

6. If there were additional validation requirements for user input, I would have considered separating out the validation logic from the 'FXCalculatorConsoleService', into its own dedicated class.

7. Similarly, if the types of errors cases at the Application layer increased, I perhaps would consider replacing the exception throwing/catching approach, with a more comprehensive response object that contained error information for the presentation layer to act on.

## Production - Things to consider
I would consider several options for a production environment:

1. Ordinarily I would include a pre-commit hook in an exercise such as this, to run the unit tests before attempting to commit, to prevent erroneos code from making its way into the repository. However as there were instructions to not share the solution via Git repository, I skipped this inclusion. It should go without saying that in a production setting, it would be necessary to run those build checks and run the unit tests in a proper deployment pipeline, as a gate-keeper for deployment to any environment. 

2. My assumption is that this solution would sit behind an API server in a production environment. Given the light-weight nature of this solution (in particular, no database with no Entity Framework models to deal with), I would consider an AWS Lambda (or Azure Functions equiavalent) sitting behind an API Gateway, to run it in production.

3. I would consider greater logging of incoming requests, to provide increased visibilty in case of errors that arise.

4. Defensive coding to catch failures at the datastore level.

5. For a production data store, seeing as I have already gone down the route of using JSON objects to store the data, using a NoSQL database like MongoDB would be an attractive option. Additionally, during the processing of a cross rate, I have opted to cycle through the various pairs using a while loop. If I were to then use an actual database, then I would consider caching the data in-memory, to avoid repeated external calls to the database.