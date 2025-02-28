
using Lexer;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches the definition symbol used in Backus-Naur Form (BNF) notation.
/// </summary>
internal class DefinitionRule : ExactMatchRule<Token>
{
    /// <summary>
    /// The token produced when the definition symbol is matched.
    /// </summary>
    private static readonly Token DefinitionToken = new(TokenType.DefinitionSymbol, DefinitionSymbol.AsMemory());

    /// <summary>
    /// The definition symbol used in BNF notation, which is "::=".
    /// </summary>
    private const string DefinitionSymbol = "::=";

    /// <summary>
    /// Initializes a new instance of the DefinitionRule class.
    /// </summary>
    public DefinitionRule() : base(DefinitionSymbol.AsMemory(), DefinitionToken) { }
}

