
using System.Buffers;
using System.Security.Principal;
using Lexer;

namespace Backus_Naur_Form.Rules.Basics;

/// <summary>
/// An abstract base class for lexer rules that match a sequence of characters delimited by specific start and end characters.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer rule.</typeparam>
public abstract class CharDelimitedSequenceRule<TToken> : Rule<TToken>
{
    /// <summary>
    /// Gets the starting character that signifies the beginning of the match.
    /// </summary>
    protected char Start { get; init; }

    /// <summary>
    /// Gets the ending character that signifies the end of the match.
    /// </summary>
    protected char End { get; init; }

    /// <summary>
    /// Gets the set of characters that are not allowed within the matched sequence.
    /// </summary>
    protected SearchValues<char> NotAllowedChars { get; init; }

    /// <summary>
    /// Initializes a new instance of the CharDelimitedSequenceRule class with the specified start and end characters.
    /// </summary>
    /// <param name="start">The starting character.</param>
    /// <param name="end">The ending character.</param>
    /// <param name="notAllowedChars">The characters that are not allowed within the matched sequence.</param>
    /// <param name="returnTokenOnMatch">True to return the token on match; false to skip the token.</param>
    /// <exception cref="ArgumentException">Thrown if the end character is not in the not allowed characters.</exception>
    protected CharDelimitedSequenceRule(char start, char end, ReadOnlySpan<char> notAllowedChars, bool returnTokenOnMatch = true) : base(returnTokenOnMatch)
    {
        Start = start;
        End = end;
        NotAllowedChars = SearchValues.Create(notAllowedChars);

        if (!notAllowedChars.Contains(End)) { throw new ArgumentException("End character must be in the not allowed characters."); }
    }

    /// <summary>
    /// Generates a token from the matched input text.
    /// </summary>
    /// <param name="matchedInput">The matched input text.</param>
    /// <returns>The generated token.</returns>
    protected abstract TToken GenerateToken(ReadOnlyMemory<char> matchedInput);

    /// <summary>
    /// Attempts to match the input text with the specified start and end characters.
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
        ReadOnlySpan<char> inputSpan = input.Span;

        // Check if the text starts with the start character
        if (inputSpan[0] != Start) { return new(isMatchFound: false, matchedTextLength: 0, isEndOfFile: false, generatedToken: default!); }

        // Move past the start character
        ReadOnlySpan<char> tempSpan = inputSpan[1..];
        int index = tempSpan.IndexOfAny(NotAllowedChars);
        int matchedLength = 0;

        while (index > 0 && tempSpan[index - 1] == '\\')
        {
            if (index > 1 && tempSpan[index - 2] == '\\') { break; }
            matchedLength += index + 1;
            tempSpan = tempSpan[(index + 1)..];
            index = tempSpan.IndexOfAny(NotAllowedChars);
        }

        // Ensure the end character is found
        if (index is -1) { throw new InvalidOperationException($"Sequence end char '{End}' not found."); }

        matchedLength += index;
        
        char invalidChar = inputSpan[matchedLength + 1];
        if (invalidChar != End)
        {
            ReadOnlySpan<char> matchedSpan = inputSpan.Slice(start: 1, length: matchedLength);
            throw new InvalidOperationException(
                $"Invalid sequence: {matchedSpan}({invalidChar})?.");
        }

        // Generate the token from the matched input text
        TToken token = GenerateToken(input.Slice(start: 1, length: matchedLength));

        // Calculate the total length of the matched text
        int length = matchedLength + 2; // Include start and end characters

        return new(
            isMatchFound: true,
            matchedTextLength: length,
            isEndOfFile: input.Length == length,
            generatedToken: token);
    }
}