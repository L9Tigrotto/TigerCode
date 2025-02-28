
using Backus_Naur_Form;
using Lexer;


// Define the file to be processed by the lexer
FileInfo fileInfo = new("BNF.lexer");

// Create a skip rule for the BNF lexer to ignore whitespace characters (space, tab, newline, carriage return)
LexerRule<Token> skip = LexerRuleManager.CreateSkipRule();

// Create an end-of-file rule for the BNF lexer to detect the end of the input file
LexerRule<Token> endOfFile = LexerRuleManager.CreateEndOfFileRule();

// Create a list of rules for the BNF lexer, including rules for non-terminal symbols, terminal symbols, comments, and special symbols
List<LexerRule<Token>> rules = LexerRuleManager.CreateRules();

// Initialize the lexer with the file, skip rule, end-of-file rule, and the list of rules
Lexer<Token> lexer = Lexer<Token>.From(fileInfo, skip, endOfFile, rules);

Token token;
do
{
    try
    {
        // Get the next token from the lexer
        token = lexer.NextToken();

        // Print the token to the console
        Console.WriteLine(token);

        // Wait for a key press before continuing
        Console.ReadKey(intercept: true);
    }
    catch (Exception e)
    {
        // Handle any exceptions that occur during tokenization
        Console.WriteLine(e);
        return;
    }
} while (token.TokenType is not TokenType.EndOfFile); // Continue processing until the end-of-file token is encountered
