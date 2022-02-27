# ANZx-FXCalculator
ANZx - Programming Exercise – FX Calculator


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
    - Amounts: For zero amounts, there's a divide by zero issue we need to skirt around.
    - Currencies: we have instructions on how to handle the case of not being able to find a currency pair.

    c. What if a currency entered doesn't exist?
    - Potentially we could treat it in a similar manner to not being able to find a currency pair. We can say that currency was not found.

    d. Do we need to validate transactions with a negative amount?
    - Does it matter? Its not like attempting to withdraw a negative amount from a bank account.




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
14. First run of the happy path unit tests for direct pairs picked up that the cross table entry for CNY/USD (direct) - USD/CNY (inverted) aren't playing nice. 
15. Googled the current price of US dollars to Chinese Yuan, and noticed it's in the region of ~6.33, so the direct feed data is in the range for a USD/CNY pair, therefore the table guidance is wrong for this pair. CNY/USD should be inverted, and USD/CNY should be direct.
    - Decision to be made: do we fix the table, or account for 'errors' in the table in code?
    - Guidance in the instructions said to use the table provided. Assumption is this is a deliberate error.
    - Will go with keeping the table as is, and make the direct/inverted rate lookup more robust.
16. We can handle the issue of incorrect table guidance by doing away with the distinction between direct and inverted, as far as the service, factory, and currency exchangers are concerned. In effect, the parameter for direct/inverted as seen in the table, is now meaningless. Instead, we can work out the correct rate when we ask for the exchange pair from the 'CurrencyLoader'.
17. The CurrencyLoader can work out if a pair it has been provided is inverted or not (when viewed against the currency pair rate table), and switch up the rate (1/rate) on the fly.
15. Tidied up the unit tests.
16. Created 'FXCalculator.Console' project to contain the console app to interact through.
17. Created 'FXCalculatorConsoleService' to handle validation of user input, and act as the main entry point to the 'FXCalculatorService'.
18. Went with a RegEx approach to validate user input - seemed like it would simplify things.
16. Created the Dockerfile, as an option for running the solution.
17. Filled out the rest of the README.md