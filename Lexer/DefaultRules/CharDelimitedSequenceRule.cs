
using System.Buffers;

namespace Lexer.DefaultRules;

public abstract class CharDelimitedSequenceRule<TToken> : LexerRule<TToken>
{
    protected char Start { get; init; }

    protected char End { get; init; }

    protected SearchValues<char> NotAllowedChars { get; init; }

    protected abstract TToken GenerateToken(ReadOnlyMemory<char> matchedInput);

    protected CharDelimitedSequenceRule(char start, char end, ReadOnlySpan<char> notAllowedChars, bool returnTokenOnMatch = true) : base(returnTokenOnMatch)
    {
        Start = start;
        End = end;
        NotAllowedChars = SearchValues.Create(notAllowedChars);

        if (!notAllowedChars.Contains(End)) { throw new ArgumentException("End character must be in the not allowed characters."); }
    }

    public override MatchResult<TToken> Match(ReadOnlyMemory<char> input)
    {
        if (input.IsEmpty) { return new(isMatchFound: false, matchedTextLength: 0, isEndOfFile: false, generatedToken: default!); }
        ReadOnlySpan<char> inputSpan = input.Span;

        // Check if the text starts with the start character
        if (inputSpan[0] != Start) { return new(isMatchFound: false, matchedTextLength: 0, isEndOfFile: false, generatedToken: default!); }

        // Move past the start character
        inputSpan = inputSpan[1..];
        int index = inputSpan.IndexOfAny(NotAllowedChars);

        // Ensure the end character is found
        if (index is -1) { throw new InvalidOperationException($"Sequence end char '{End}' not found."); }

        char invalidChar = inputSpan[index];
        if (invalidChar != End)
        {
            throw new InvalidOperationException(
                $"Invalid sequence: {inputSpan[..index]}({invalidChar})?.");
        }

        TToken token = GenerateToken(input.Slice(start: 1, length: index));
        int length = index + 2; // Include start and end characters
        return new(
            isMatchFound: true,
            matchedTextLength: length,
            isEndOfFile: input.Length == length,
            generatedToken: token);
    }
}