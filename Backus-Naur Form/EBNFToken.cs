
using Lexer;

namespace Backus_Naur_Form;

/// <summary>
/// Represents the type of tokens in an Extended Backus-Naur Form (EBNF) grammar.
/// </summary>
public enum EBNFTokenType
{
    /// <summary>
    /// Represents an empty token.
    /// </summary>
    Empty,

    /// <summary>
    /// Represents whitespace characters, such as space (' ') and tab ('\t').
    /// </summary>
    WhiteSpace,

    /// <summary>
    /// Represents a single-line comment, denoted by "//...".
    /// </summary>
    SingleLineComment,

    /// <summary>
    /// Represents a multi-line comment, enclosed within "/*...*/".
    /// </summary>
    MultiLineComment,

    NonTerminalElement,

    TerminalElement,

    DefinitionStart,

    DefinitionEnd,

    Alternative,

    OptionalElementStart,

    OptionalElementEnd,

    RepetitionElementStart,

    RepetitionElementEnd,

    GroupingElementStart,

    GroupingElementEnd,
}

/// <summary>
/// Represents a token in an Extended Backus-Naur Form (EBNF) grammar.
/// </summary>
public class EBNFToken : IToken
{
    /// <summary>
    /// Gets or sets the type of the EBNF token.
    /// </summary>
    public EBNFTokenType Type { get; set; } 

    /// <summary>
    /// Gets or sets the value of the EBNF token.
    /// </summary>
    public ReadOnlyMemory<char> Value { get; set; }

    /// <summary>
    /// Initializes a new instance of the EBNFToken class with the specified type and value.
    /// </summary>
    /// <param name="type">The type of the EBNF token.</param>
    /// <param name="value">The value of the EBNF token.</param>
    public EBNFToken(EBNFTokenType type, ReadOnlyMemory<char> value)
    {
        Type = type;
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the EBNFToken class with default values.
    /// </summary>
    public EBNFToken() : this(EBNFTokenType.Empty, ReadOnlyMemory<char>.Empty) { }

    /// <summary>
    /// Initializes a new instance of the EBNFToken class with the specified type and an empty value.
    /// </summary>
    /// <param name="type">The type of the EBNF token.</param>
    public EBNFToken(EBNFTokenType type) : this(type, ReadOnlyMemory<char>.Empty) { }

    /// <summary>
    /// Resets the EBNF token to its default state.
    /// </summary>
    public void Reset()
    {
        Type = EBNFTokenType.Empty;
        Value = ReadOnlyMemory<char>.Empty;
    }

    /// <summary>
    /// Returns a string that represents the current EBNF token.
    /// </summary>
    /// <returns>A string that represents the current EBNF token.</returns>
    public override string ToString()
    {
        return Value.IsEmpty
            ? $"Type: {Type}"
            : $"Type: {Type,-20} Value: {Value}";
    }
}
