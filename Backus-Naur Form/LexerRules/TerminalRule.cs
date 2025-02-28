
using System.Diagnostics.CodeAnalysis;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches terminal symbols in Backus-Naur Form (BNF) notation.
/// Terminal symbols are enclosed in double quotes (e.g., "symbol").
/// </summary>
[SuppressMessage("ReSharper", "GrammarMistakeInComment")]
internal class TerminalRule : DelimitedSequenceRule
{
    /// <summary>
    /// The start character for terminal symbols, which is the double quote '"'.
    /// </summary>
    private const char Start = '"';

    /// <summary>
    /// The end character for terminal symbols, which is the double quote '"'.
    /// </summary>
    private const char End = '"';

    /// <summary>
    /// An array of characters that are not allowed within terminal symbols.
    /// Currently, it includes the double quote and newline characters.
    /// </summary>
    private static readonly char[] InvalidChars = ['"', '\n'];

    /// <summary>
    /// Initializes a new instance of the TerminalRule class.
    /// </summary>
    public TerminalRule() : base(Start, End, InvalidChars, TokenType.Terminal) { }
}

