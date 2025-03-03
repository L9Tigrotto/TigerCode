
using Backus_Naur_Form.Rules.Basics;

namespace Backus_Naur_Form.Rules;

/// <summary>
/// A lexer rule that matches single-line comments in Backus-Naur Form (BNF) notation.
/// Single-line comments start with "//" and continue until the end of the line.
/// </summary>
internal class SingleLineComment : StringDelimitedSequenceRule<Token>
{
    /// <summary>
    /// The starting sequence for single-line comments, which is "//".
    /// </summary>
    private const string StartString = "//";

    /// <summary>
    /// An array of end sequences for single-line comments, which includes newline characters.
    /// </summary>
    private static readonly string[] EndStrings = ["\r\n", "\n"];

    /// <summary>
    /// Initializes a new instance of the SingleLineComment class.
    /// </summary>
    /// <remarks>
    /// In DEBUG mode, the rule returns the token on match. In non-DEBUG mode, the rule skips the token.
    /// </remarks>
#if DEBUG
    public SingleLineComment() : base(StartString.AsMemory(), EndStrings, countEndLength: false, returnTokenOnMatch: true) { }
#else
    public SingleLineComment() : base(StartString.AsMemory(), EndStrings, countEndLength: false, returnTokenOnMatch: false) { }
#endif

    /// <summary>
    /// Generates a token from the matched input text.
    /// </summary>
    /// <param name="matchedInput">The matched input text.</param>
    /// <returns>
    /// In DEBUG mode, null, as the token is not returned.
    /// In non-DEBUG mode, a token of type TokenType.SingleLineComment with the matched input text as its value.
    /// </returns>
#if DEBUG
    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return new Token(TokenType.SingleLineComment, matchedInput); }
#else
    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return null!; }
#endif
}