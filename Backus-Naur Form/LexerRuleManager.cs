
using Backus_Naur_Form.LexerRules;
using Lexer;

namespace Backus_Naur_Form;

/// <summary>
/// A static class that manages the creation of lexer rules for processing Backus-Naur Form (BNF) notation.
/// </summary>
internal static class LexerRuleManager
{
    public static List<LexerRule<Token>> CreateRules(out NewLineRule newLineRule)
    {
        newLineRule = new NewLineRule();
        return
        [
            // return token on match = false

            // return token on match only if debugging
            newLineRule,                // Matches new line characters (e.g., \n)
            new WhiteSpaceRule(),       // Matches whitespace characters (e.g., ' ', '\t')
            new SingleLineComment(),    // Matches single-line comments (e.g., // text)

            // return token on match = true
            new TerminalRule(),         // Matches terminal symbols (e.g., "value")
            new SeparationRule(),       // Matches the separation symbol (e.g., |)
            new NonTerminalRule(),      // Matches non-terminal symbols (e.g., <name>)
            new DefinitionRule(),       // Matches the definition symbol (e.g., ::=)
            new TerminationRule(),      // Matches the termination symbol (e.g., ;)
        ];
    }
}