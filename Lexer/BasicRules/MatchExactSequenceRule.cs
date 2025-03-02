
namespace Lexer.BasicRules;

/// <summary>
/// A lexer rule that matches an exact sequence of characters and produces a specified token.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer rule.</typeparam>
public class MatchExactSequenceRule<TToken> : LexerRule<TToken>
{
    /// <summary>
    /// Gets the sequence of characters to match in the input text.
    /// </summary>
    protected ReadOnlyMemory<char> Sequence { get; init; }

    /// <summary>
    /// Gets the token produced when the exact match is found.
    /// </summary>
    protected TToken Token { get; init; }

    /// <summary>
    /// Initializes a new instance of the MatchExactSequenceRule class with the specified sequence and token.
    /// </summary>
    /// <param name="sequence">The sequence of characters to match.</param>
    /// <param name="token">The token to produce when the sequence is found.</param>
    /// <param name="returnTokenOnMatch">True to return the token on match; false to skip the token.</param>
    /// <exception cref="ArgumentException">Thrown if the sequence is empty.</exception>
    public MatchExactSequenceRule(ReadOnlyMemory<char> sequence, TToken token, bool returnTokenOnMatch = true) : base(returnTokenOnMatch)
    {
        if (sequence.IsEmpty) { throw new ArgumentException("Sequence is empty."); }
        Sequence = sequence;
        Token = token;
    }

    /// <summary>
    /// Attempts to match the input text with the exact sequence of characters.
    /// </summary>
    /// <param name="input">The input text to match.</param>
    /// <returns>
    /// A MatchResult containing information about whether a match was found,
    /// the length of the matched text, whether the end of the file has been reached,
    /// and the generated token if a match was found.
    /// </returns>
    public override MatchResult<TToken> Match(ReadOnlyMemory<char> input)
    {
        if (input.IsEmpty) { return new(isMatchFound: false, matchedTextLength: 0, isEndOfFile: false, generatedToken: default!); }
        ReadOnlySpan<char> sequence = Sequence.Span;
        ReadOnlySpan<char> inputSpan = input.Span;

        // Check if the input text starts with the exact sequence to match
        if (sequence.Length <= inputSpan.Length)
        {
            inputSpan = inputSpan[..sequence.Length];

            if (inputSpan.SequenceEqual(sequence))
            {
                return new(
                    isMatchFound: true,
                    matchedTextLength: sequence.Length,
                    isEndOfFile: input.Length == Sequence.Length,
                    generatedToken: Token);
            }
        }

        return new(isMatchFound: false, matchedTextLength: 0, isEndOfFile: false, generatedToken: default!);
    }
}