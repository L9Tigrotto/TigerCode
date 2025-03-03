
using Backus_Naur_Form.Rules.Basics;

namespace Backus_Naur_Form.Rules;

internal class AttributeRule : CharDelimitedSequenceRule<Token>
{
    private const char StartChar = '[';

    private const char EndChar = ']';

    private static readonly char[] InvalidChars =
    [
        ' ', '\t', '\n', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '+', '=',
        '[', ']', '{', '}', '|', '\\', ':', ';', '"', '\'', '<', '>', '?', '/', '~', ','
    ];

    public AttributeRule() : base(StartChar, EndChar, InvalidChars) { }

    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return new Token(TokenType.Attribute, matchedInput); }
}
