
using Lexer;

namespace Backus_Naur_Form.Rules.Basics;

/// <summary>
/// An abstract base class for lexer rules that match a string sequence delimited by specific start and end characters.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer rule.</typeparam>
public abstract class StringDelimitedSequenceRule<TToken> : Rule<TToken>
{
    /// <summary>
    /// Gets the starting sequence of characters that signifies the beginning of the match.
    /// </summary>
    protected ReadOnlyMemory<char> Start { get; init; }

    /// <summary>
    /// Gets the ending sequence of characters that signifies the end of the match.
    /// </summary>
    protected ReadOnlyMemory<string> End { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the length of the end sequence should be included in the matched text length.
    /// </summary>
    public bool CountEndLength { get; set; }

    /// <summary>
    /// Initializes a new instance of the StringDelimitedSequenceRule class with the specified start and end sequences.
    /// </summary>
    /// <param name="start">The starting sequence of characters.</param>
    /// <param name="end">The ending sequence of characters.</param>
    /// <param name="countEndLength">True if the length of the end sequence should be included in the matched text length; otherwise, false.</param>
    /// <param name="returnTokenOnMatch">True to return the token on match; false to skip the token.</param>
    protected StringDelimitedSequenceRule(ReadOnlyMemory<char> start, ReadOnlyMemory<string> end, bool countEndLength, bool returnTokenOnMatch = true) : base(returnTokenOnMatch)
    {
        Start = start;
        End = end;
        CountEndLength = countEndLength;
    }

    /// <summary>
    /// Generates a token from the matched input text.
    /// </summary>
    /// <param name="matchedInput">The matched input text.</param>
    /// <returns>The generated token.</returns>
    protected abstract TToken GenerateToken(ReadOnlyMemory<char> matchedInput);

    /// <summary>
    /// Attempts to match the input text with the specified start and end sequences.
    /// </summary>
    /// <param name="input">The input text to match.</param>
    /// <returns>A MatchResult containing information about the match operation.</returns>
    public override MatchResult<TToken> Match(ReadOnlyMemory<char> input)
    {
        if (input.IsEmpty) { return new(isMatchFound: false, matchedTextLength: 0, isEndOfFile: false, generatedToken: default!); }
        ReadOnlySpan<char> inputSpan = input.Span;
        ReadOnlySpan<char> startSpan = Start.Span;

        // Check if the text starts with the start text
        if (startSpan.Length > inputSpan.Length || !inputSpan[..startSpan.Length].SequenceEqual(startSpan))
        {
            return new(isMatchFound: false, matchedTextLength: 0, isEndOfFile: false, generatedToken: default!);
        }

        // Move past the start text
        inputSpan = inputSpan[startSpan.Length..];

        int lessDistantIndex = -1;
        int lessDistantLength = int.MaxValue;

        ReadOnlySpan<string> endSpan = End.Span;
        for (int i = 0; i < End.Length; i++)
        {
            int distance = inputSpan.IndexOf(endSpan[i]);
            if (distance < lessDistantLength)
            {
                lessDistantIndex = i;
                lessDistantLength = distance;
            }
        }

        // Ensure the end character is found
        if (lessDistantIndex is -1) { throw new InvalidOperationException($"Sequence end not found."); }

        // Generate the token from the matched input text
        TToken token = GenerateToken(input.Slice(start: Start.Length, length: lessDistantLength));

        // Calculate the total length of the matched text
        int length = CountEndLength
            ? Start.Length + lessDistantLength + endSpan[lessDistantIndex].Length
            : Start.Length + lessDistantLength;

        return new(
            isMatchFound: true,
            matchedTextLength: length,
            isEndOfFile: input.Length == length,
            generatedToken: token);
    }
}