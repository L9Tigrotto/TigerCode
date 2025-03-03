
using Backus_Naur_Form.Rules.Basics;

namespace Backus_Naur_Form.Rules;

/// <summary>
/// A lexer rule that matches single-line comments in Backus-Naur Form (BNF) notation.
/// Single-line comments start with "//" and continue until the end of the line.
/// </summary>
internal class SingleLineCommentRule : StringDelimitedSequenceRule<Token>
{
    /// <summary>
    /// The starting sequence for single-line comments, which is "//".
    /// </summary>
    private const string StartString = "//";

    /// <summary>
    /// An array of end sequences for single-line comments, which includes newline characters.
    /// </summary>
    private static readonly string[] EndStrings = ["\r\n", "\n"];

    public SingleLineCommentRule() : base(StartString.AsMemory(), EndStrings, countEndLength: false, returnTokenOnMatch: false) { }

    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return null!; }
}