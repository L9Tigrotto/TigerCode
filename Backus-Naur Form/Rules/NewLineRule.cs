using Backus_Naur_Form.Rules.Basics;
using Lexer;

namespace Backus_Naur_Form.Rules;

/// <summary>
/// A lexer rule that matches new line characters in Backus-Naur Form (BNF) notation.
/// New line characters include '\r\n' and '\n'.
/// </summary>
internal class NewLineRule : MatchExactSequencesRule<Token>
{
    /// <summary>
    /// Gets or sets the current line number in the input text.
    /// </summary>
    public int CurrentLine { get; protected set; } = 1;

    /// <summary>
    /// An array of new line sequences that this rule matches.
    /// </summary>
    private static readonly string[] NewLineSequences = ["\r\n", "\n"];

    /// <summary>
    /// The token produced when a new line sequence is matched.
    /// </summary>
    private static readonly Token NewLineToken = new(TokenType.NewLine, @"\r\n".AsMemory());

    /// <summary>
    /// Matches new line characters in the input text and updates the current line number.
    /// </summary>
    /// <param name="input">The input text to match.</param>
    /// <returns>A MatchResult containing information about the match operation.</returns>
    public override MatchResult<Token> Match(ReadOnlyMemory<char> input)
    {
        MatchResult<Token> result = base.Match(input);
        if (result.IsMatchFound) { CurrentLine++; }
        return result;
    }

    /// <summary>
    /// Initializes a new instance of the NewLineRule class.
    /// </summary>
    /// <remarks>
    /// In DEBUG mode, the rule returns the token on match. In non-DEBUG mode, the rule skips the token.
    /// </remarks>
#if DEBUG
    public NewLineRule() : base(NewLineSequences.AsMemory(), NewLineToken, returnTokenOnMatch: true) { }
#else
    public NewLineRule() : base(NewLineSequences.AsMemory(), NewLineToken, returnTokenOnMatch: false) { }
#endif
}

