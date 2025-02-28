
namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches single-line comments in Backus-Naur Form (BNF) notation.
/// Single-line comments start with "//" and continue until the end of the line.
/// </summary>
internal class SingleLineComment : DelimitedSequenceWithSequencesRule
{
    /// <summary>
    /// The start sequence for single-line comments, which is "//".
    /// </summary>
    private const string Start = "//";

    /// <summary>
    /// The end character for single-line comments, which is the newline character '\n'.
    /// </summary>
    private const char End = '\n';

    /// <summary>
    /// An array of characters that are not allowed within single-line comments.
    /// Currently, it includes the newline character and double quotes.
    /// </summary>
    private static readonly char[] InvalidChars = ['"', '\n'];

    /// <summary>
    /// Initializes a new instance of the SingleLineComment class.
    /// </summary>
    public SingleLineComment() : base(Start.AsMemory(), End, InvalidChars, TokenType.SingleLineComment) { }
}