
using System.Diagnostics.CodeAnalysis;
using Lexer.DefaultRules;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches terminal symbols in Backus-Naur Form (BNF) notation.
/// Terminal symbols are enclosed in double quotes (e.g., "symbol").
/// </summary>
internal class TerminalRule : CharDelimitedSequenceRule<Token>
{
    /// <summary>
    /// The start character for terminal symbols, which is the double quote '"'.
    /// </summary>
    private const char StartChar = '"';

    /// <summary>
    /// The end character for terminal symbols, which is the double quote '"'.
    /// </summary>
    private const char EndChar = '"';

    /// <summary>
    /// An array of characters that are not allowed within terminal symbols.
    /// Currently, it includes the double quote and newline characters.
    /// </summary>
    private static readonly char[] InvalidChars = ['"', '\n'];

    /// <summary>
    /// Initializes a new instance of the TerminalRule class.
    /// </summary>
    public TerminalRule() : base(StartChar, EndChar, InvalidChars) { }

    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return new Token(TokenType.Terminal, matchedInput); }
}

