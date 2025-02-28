
using System.Diagnostics.CodeAnalysis;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches non-terminal symbols in Backus-Naur Form (BNF) notation.
/// Non-terminal symbols are enclosed in angle brackets (e.g., &lt;symbol&gt;).
/// </summary>
[SuppressMessage("ReSharper", "InvalidXmlDocComment")]
internal class NonTerminalRule : DelimitedSequenceRule
{
    /// <summary>
    /// The start character for non-terminal symbols, which is '<'.
    /// </summary>
    private const char Start = '<';

    /// <summary>
    /// The end character for non-terminal symbols, which is '>'.
    /// </summary>
    private const char End = '>';

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
    public NonTerminalRule() : base(Start, End, InvalidChars, TokenType.NonTerminal) { }
}
