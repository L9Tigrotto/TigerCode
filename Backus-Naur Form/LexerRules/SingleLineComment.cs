
using Lexer.DefaultRules;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches single-line comments in Backus-Naur Form (BNF) notation.
/// Single-line comments start with "//" and continue until the end of the line.
/// </summary>
internal class SingleLineComment : StringDelimitedSequenceRule<Token>
{
    private const string StartString = "//";

    private static readonly string[] EndStrings = ["\r\n", "\n"];

#if DEBUG
    public SingleLineComment() : base(StartString.AsMemory(), EndStrings, countEndLength: false, returnTokenOnMatch: true) { }

    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return new Token(TokenType.SingleLineComment, matchedInput); }
#else
    public SingleLineComment() : base(StartString.AsMemory(), EndStrings, countEndLength: false, returnTokenOnMatch: false) { }

    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return null!; }
#endif
}