
using Lexer.BasicRules;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches whitespace characters (space and tab) and produces a whitespace token.
/// </summary>
internal class WhiteSpaceRule : MatchExactSequencesRule<Token>
{
    /// <summary>
    /// An array of whitespace sequences that this rule matches.
    /// </summary>
    private static readonly string[] WhiteSpaceSequences = [" ", "\t"];

    /// <summary>
    /// The token produced when a whitespace sequence is matched.
    /// </summary>
    private static readonly Token WhiteSpaceToken = new(TokenType.WhiteSpace, WhiteSpaceSequences[0].AsMemory());

    /// <summary>
    /// Initializes a new instance of the WhiteSpaceRule class.
    /// </summary>
    /// <remarks>
    /// In DEBUG mode, the rule returns the token on match. In non-DEBUG mode, the rule skips the token.
    /// </remarks>
#if DEBUG
    public WhiteSpaceRule() : base(WhiteSpaceSequences.AsMemory(), WhiteSpaceToken, returnTokenOnMatch: true) { }

#else
    public WhiteSpaceRule() : base(WhiteSpaceSequences.AsMemory(), WhiteSpaceToken, returnTokenOnMatch: false) { }
#endif
}