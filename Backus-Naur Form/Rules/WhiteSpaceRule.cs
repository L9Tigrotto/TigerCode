
using Backus_Naur_Form.Rules.Basics;

namespace Backus_Naur_Form.Rules;

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

    public WhiteSpaceRule() : base(WhiteSpaceSequences.AsMemory(), WhiteSpaceToken, returnTokenOnMatch: false) { }
}