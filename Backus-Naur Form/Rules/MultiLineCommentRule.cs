
using Backus_Naur_Form.Rules.Basics;

namespace Backus_Naur_Form.Rules;

public class MultiLineCommentRule : StringDelimitedSequenceRule<Token>
{
    private const string StartString = "/*";
    private static readonly string[] EndStrings = ["*/"];

    public MultiLineCommentRule() : base(StartString.AsMemory(), EndStrings, countEndLength: true, returnTokenOnMatch: false) { }

    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return null!; }
}

