
using Backus_Naur_Form.Rules.Basics;

namespace Backus_Naur_Form.Rules;

public class MultiLineCommentRule : StringDelimitedSequenceRule<Token>
{
    private const string StartString = "/*";
    private static readonly string[] EndStrings = ["*/"];

#if DEBUG
    public MultiLineCommentRule() : base(StartString.AsMemory(), EndStrings, countEndLength: true, returnTokenOnMatch: true) { }
#else
    public MultiLineCommentRule() : base(StartString.AsMemory(), EndStrings, countEndLength: true, returnTokenOnMatch: false) { }
#endif

#if DEBUG
    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return new Token(TokenType.MultiLineComment, matchedInput); }
#else
    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return null!; }
#endif
}

