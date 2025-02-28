
using Lexer;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches the separation symbol used in Backus-Naur Form (BNF) notation.
/// The separation symbol is used to denote alternatives in a production rule (e.g., &lt;symbol&gt; ::= A | B).
/// </summary>
internal class SeparationRule : ExactMatchRule<Token>
{
    /// <summary>
    /// The token produced when the separation symbol is matched.
    /// </summary>
    private static readonly Token SeparationToken = new(TokenType.DefinitionSymbol, SeparationSymbol.AsMemory());

    /// <summary>
    /// The separation symbol used in BNF notation, which is "|".
    /// </summary>
    private const string SeparationSymbol = "|";

    /// <summary>
    /// Initializes a new instance of the SeparationRule class.
    /// </summary>
    public SeparationRule() : base(SeparationSymbol.AsMemory(), SeparationToken) { }
}

