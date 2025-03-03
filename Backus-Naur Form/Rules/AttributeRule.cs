

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

    /// <summary>
    /// Generates a token from the matched input text.
    /// </summary>
    /// <param name="matchedInput">The matched input text.</param>
    /// <returns>A token of type TokenType.Terminal with the matched input text as its value.</returns>
    protected override Token GenerateToken(ReadOnlyMemory<char> matchedInput) { return new Token(TokenType.Attribute, matchedInput); }
}
