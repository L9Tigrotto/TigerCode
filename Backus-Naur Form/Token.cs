
namespace Backus_Naur_Form;

/// <summary>
/// Enumeration representing the types of tokens in Backus-Naur Form (BNF) notation.
/// </summary>
public enum TokenType
{
    /// <summary>
    /// Represents a new line character, including '\n' and '\r'.
    /// </summary>
    NewLine,            // \n\r

    /// <summary>
    /// Represents a white space character, including space (' ') and tab ('\t').
    /// </summary>
    WhiteSpace,         // ' ', '\t'

    /// <summary>
    /// Represents a non-terminal symbol, enclosed in angle brackets (e.g., &lt;name&gt;).
    /// </summary>
    NonTerminal,        // <{name}>

    /// <summary>
    /// Represents a terminal symbol, enclosed in double quotes (e.g., "{value}").
    /// </summary>
    Terminal,           // "{value}"

    /// <summary>
    /// Represents the definition symbol used in BNF notation (e.g., ::=).
    /// </summary>
    DefinitionSymbol,   // ::=

    /// <summary>
    /// Represents the separation symbol used to denote alternatives in a production rule (e.g., |).
    /// </summary>
    SeparationSymbol,   // |

    /// <summary>
    /// Represents the termination symbol used to denote the end of a production rule (e.g., ;).
    /// </summary>
    TerminationSymbol,  // ;

    /// <summary>
    /// Represents a single-line comment, starting with "//" and continuing until the end of the line.
    /// </summary>
    SingleLineComment,  // //{comment}
}

/// <summary>
/// Represents a token in Backus-Naur Form (BNF) notation, containing a token type and an associated value.
/// </summary>
/// <remarks>
/// Initializes a new instance of the Token class with a specified token type and value.
/// </remarks>
/// <param name="tokenType">The type of the token.</param>
/// <param name="value">The value associated with the token.</param>
public class Token(TokenType tokenType, ReadOnlyMemory<char> value)
{
    /// <summary>
    /// Gets the type of the token.
    /// </summary>
    public TokenType TokenType { get; init; } = tokenType;

    /// <summary>
    /// Gets the value associated with the token.
    /// </summary>
    public ReadOnlyMemory<char> Value { get; init; } = value;

    /// <summary>
    /// Initializes a new instance of the Token class with a specified token type and an empty value.
    /// </summary>
    /// <param name="tokenType">The type of the token.</param>
    public Token(TokenType tokenType) : this(tokenType, ReadOnlyMemory<char>.Empty) { }

    /// <summary>
    /// Returns a string representation of the token, including its type and value.
    /// </summary>
    /// <returns>A string representation of the token.</returns>
    public override string ToString()
    {
        return Value.Length != 0 ? $"Type: {TokenType,20}\tValue: '{Value}'" : $"Type: {TokenType}";
    }
}
