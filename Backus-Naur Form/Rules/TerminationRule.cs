
using Backus_Naur_Form.Rules.Basics;

namespace Backus_Naur_Form.Rules;

/// <summary>
/// A lexer rule that matches the termination symbol used in Backus-Naur Form (BNF) notation.
/// The termination symbol is used to denote the end of a production rule (e.g., &lt;symbol&gt; ::= A | B;).
/// </summary>
internal class TerminationRule : MatchExactSequenceRule<Token>
{
    /// <summary>
    /// The termination symbol used in BNF notation, which is ";".
    /// </summary>
    private const string TerminationSymbol = ";";

    /// <summary>
    /// The token produced when the termination symbol is matched.
    /// </summary>
    private static readonly Token TerminationToken = new(TokenType.TerminationSymbol, TerminationSymbol.AsMemory());

    /// <summary>
    /// Initializes a new instance of the TerminationRule class.
    /// </summary>
    public TerminationRule() : base(TerminationSymbol.AsMemory(), TerminationToken) { }
}
