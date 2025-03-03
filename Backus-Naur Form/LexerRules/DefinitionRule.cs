
using Backus_Naur_Form.Rules.Basics;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches the definition symbol used in Backus-Naur Form (BNF) notation.
/// </summary>
internal class DefinitionRule : MatchExactSequenceRule<Token>
{
    /// <summary>
    /// The definition symbol used in BNF notation, which is "::=".
    /// </summary>
    private const string DefinitionSymbol = "::=";

    /// <summary>
    /// The token produced when the definition symbol is matched.
    /// </summary>
    private static readonly Token DefinitionToken = new(TokenType.DefinitionSymbol, DefinitionSymbol.AsMemory());

    /// <summary>
    /// Initializes a new instance of the DefinitionRule class.
    /// </summary>
    public DefinitionRule() : base(DefinitionSymbol.AsMemory(), DefinitionToken) { }
}

