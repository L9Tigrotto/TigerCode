
using Lexer.DefaultRules;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches non-terminal symbols in Backus-Naur Form (BNF) notation.
/// Non-terminal symbols are enclosed in angle brackets (e.g., &lt;symbol&gt;).
/// </summary>
internal class NonTerminalRule : CharDelimitedSequenceRule<Token>
{
    /// <summary>
    /// The start character for non-terminal symbols, which is '<'.
    /// </summary>
    private const char StartChar = '<';

    /// <summary>
    /// The end character for non-terminal symbols, which is '>'.
    /// </summary>
    private const char EndChar = '>';

    /// <summary>
    /// An array of characters that are not allowed within non-terminal symbols.
    /// </summary>
    private static readonly char[] InvalidChars =
    [
        ' ', '\t', '\n', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=',
            '[', ']', '{', '}', '|', '\\', ':', ';', '"', '\'', '<', '>', '?', '/', '~', ','
    ];

    /// <summary>
    /// Initializes a new instance of the NonTerminalRule class.
    /// </summary>
    public NonTerminalRule() : base(StartChar, EndChar, InvalidChars) { }

    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return new Token(TokenType.NonTerminal, matchedInput); }
}
