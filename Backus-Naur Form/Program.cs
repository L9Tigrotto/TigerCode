
using Backus_Naur_Form;
using Backus_Naur_Form.Rules;
using Lexer;

// Define the file to be processed by the lexer
FileInfo fileInfo = new("BNF.lexer");

// Create a list of rules for the BNF lexer, including rules for non-terminal symbols, terminal symbols, comments, and special symbols
LexerRule<Token>[] rules = LexerRuleManager.CreateRules(out NewLineRule newLineRule);

// Initialize the lexer with the file, skip rule, end-of-file rule, and the list of rules
Lexer<Token> lexer = Lexer<Token>.From(fileInfo, rules);

do
{
    try
    {
        // Get the next token from the lexer
        Token token = lexer.NextToken(out bool endOfFile);

        if (endOfFile)
        {
            Console.WriteLine("End of file.");
            break;
        }

        // Print the token to the console
        Console.WriteLine(token);

        // Wait for a key press before continuing
        //Console.ReadKey(intercept: true);
    }
    catch (Exception e)
    {
        // Handle any exceptions that occur during tokenization
        Console.WriteLine($"Error at line {newLineRule.CurrentLine}\n{e}");
        return;
    }
} while (true);
