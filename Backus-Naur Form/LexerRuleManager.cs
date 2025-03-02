
using Backus_Naur_Form.LexerRules;
using Lexer;

namespace Backus_Naur_Form;

/// <summary>
/// A static class that manages the creation of lexer rules for processing Backus-Naur Form (BNF) notation.
/// </summary>
internal static class LexerRuleManager
{
    /// <summary>
    /// Creates an array of lexer rules for processing BNF notation.
    /// </summary>
    /// <param name="newLineRule">The rule for matching new line characters.</param>
    /// <returns>An array of LexerRule instances for various BNF constructs.</returns>
    public static LexerRule<Token>[] CreateRules(out NewLineRule newLineRule)
    {
        // Initialize rules for different BNF constructs
        newLineRule = new NewLineRule();
        WhiteSpaceRule whiteSpaceRule = new();
        SingleLineComment singleLineComment = new();
        NonTerminalRule nonTerminalRule = new();
        TerminalRule terminalRule = new();
        SeparationRule separationRule = new();
        DefinitionRule definitionRule = new();
        TerminationRule terminationRule = new();

        // Define subsequent rules for each rule to optimize matching
        newLineRule.SubsequentRules = [newLineRule, whiteSpaceRule, singleLineComment, nonTerminalRule];
        whiteSpaceRule.SubsequentRules = [whiteSpaceRule, separationRule, definitionRule, terminalRule, nonTerminalRule];
        singleLineComment.SubsequentRules = [newLineRule];
        nonTerminalRule.SubsequentRules = [whiteSpaceRule, definitionRule, nonTerminalRule, terminationRule];
        terminalRule.SubsequentRules = [whiteSpaceRule, separationRule, nonTerminalRule];
        separationRule.SubsequentRules = [whiteSpaceRule, terminalRule, nonTerminalRule];
        definitionRule.SubsequentRules = [whiteSpaceRule, terminalRule, nonTerminalRule];
        terminationRule.SubsequentRules = [newLineRule, whiteSpaceRule, nonTerminalRule];

        return
        [
            // Rules that do not return a token on match
            newLineRule,                // Matches new line characters (e.g., \n)
            new WhiteSpaceRule(),       // Matches whitespace characters (e.g., ' ', '\t')
            new SingleLineComment(),    // Matches single-line comments (e.g., // text)

            // Rules that return a token on match
            new TerminalRule(),         // Matches terminal symbols (e.g., "value")
            new SeparationRule(),       // Matches the separation symbol (e.g., |)
            new NonTerminalRule(),      // Matches non-terminal symbols (e.g., <name>)
            new DefinitionRule(),       // Matches the definition symbol (e.g., ::=)
            new TerminationRule(),      // Matches the termination symbol (e.g., ;)
        ];
    }
}