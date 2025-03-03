
using Backus_Naur_Form.Rules;
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
        newLineRule = new();
        WhiteSpaceRule whiteSpaceRule = new();
        SingleLineCommentRule singleLineCommentRule = new();
        MultiLineCommentRule multiLineCommentRule = new();
        AttributeRule attributeRule = new();
        NonTerminalRule nonTerminalRule = new();
        TerminalRule terminalRule = new();
        SeparationRule separationRule = new();
        DefinitionRule definitionRule = new();
        TerminationRule terminationRule = new();

        // Define subsequent rules for each rule to optimize matching
        newLineRule.SubsequentRules = [newLineRule, attributeRule, whiteSpaceRule, nonTerminalRule, singleLineCommentRule, multiLineCommentRule];
        whiteSpaceRule.SubsequentRules = [whiteSpaceRule, attributeRule, separationRule, definitionRule, terminalRule, singleLineCommentRule, multiLineCommentRule];
        singleLineCommentRule.SubsequentRules = [newLineRule];
        multiLineCommentRule.SubsequentRules = [newLineRule];
        attributeRule.SubsequentRules = [newLineRule, nonTerminalRule];
        nonTerminalRule.SubsequentRules = [whiteSpaceRule, terminationRule];
        terminalRule.SubsequentRules = [whiteSpaceRule];
        separationRule.SubsequentRules = [whiteSpaceRule];
        definitionRule.SubsequentRules = [whiteSpaceRule];
        terminationRule.SubsequentRules = [newLineRule];

        return
        [
            // Rules that do not return a token on match
            newLineRule,                // Matches new line characters (e.g., \n)
            new WhiteSpaceRule(),       // Matches whitespace characters (e.g., ' ', '\t')
            new SingleLineCommentRule(),    // Matches single-line comments (e.g., // text)
            new MultiLineCommentRule(),     // Matches multi-line comments (e.g., /* text */)

            // Rules that return a token on match
            new TerminalRule(),         // Matches terminal symbols (e.g., "value")
            new SeparationRule(),       // Matches the separation symbol (e.g., |)
            new NonTerminalRule(),      // Matches non-terminal symbols (e.g., <name>)
            new DefinitionRule(),       // Matches the definition symbol (e.g., ::=)
            new TerminationRule(),      // Matches the termination symbol (e.g., ;)
        ];
    }
}