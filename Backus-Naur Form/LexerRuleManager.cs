using Backus_Naur_Form.LexerRules;
using Lexer;

namespace Backus_Naur_Form;

/// <summary>
/// A static class that manages the creation of lexer rules for processing Backus-Naur Form (BNF) notation.
/// </summary>
public static class LexerRuleManager
{
    /// <summary>
    /// Creates a lexer rule to skip whitespace characters (space, tab, newline, carriage return).
    /// </summary>
    /// <returns>A SkipRule instance configured to skip whitespace characters.</returns>
    public static LexerRule<Token> CreateSkipRule() { return new SkipRule<Token>([" ", "\t", "\n", "\r"]); }

    /// <summary>
    /// Creates a lexer rule to detect the end of the file.
    /// </summary>
    /// <returns>An EndOfFileRule instance that produces an EndOfFile token.</returns>
    public static LexerRule<Token> CreateEndOfFileRule() { return new EndOfFileRule<Token>(new Token(TokenType.EndOfFile)); }

    /// <summary>
    /// Creates a list of lexer rules for processing BNF notation.
    /// </summary>
    /// <returns>A list of LexerRule instances for various BNF constructs.</returns>
    public static List<LexerRule<Token>> CreateRules()
    {
        return
        [
            new NonTerminalRule(),      // Matches non-terminal symbols (e.g., <name>)
            new TerminalRule(),         // Matches terminal symbols (e.g., "value")
            new SingleLineComment(),    // Matches single-line comments (e.g., // text)
            new DefinitionRule(),       // Matches the definition symbol (e.g., ::=)
            new SeparationRule(),       // Matches the separation symbol (e.g., |)
            new TerminationRule(),      // Matches the termination symbol (e.g., ;)
        ];
    }
}