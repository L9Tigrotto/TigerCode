
namespace Lexer.DefaultRules;

public abstract class StringDelimitedSequenceRule<TToken> : LexerRule<TToken>
{
    protected ReadOnlyMemory<char> Start { get; init; }

    protected ReadOnlyMemory<string> End { get; init; }

    public bool CountEndLength { get; set; }

    protected abstract TToken GenerateToken(ReadOnlyMemory<char> matchedInput);

    protected StringDelimitedSequenceRule(ReadOnlyMemory<char> start, ReadOnlyMemory<string> end, bool countEndLength, bool returnTokenOnMatch = true) : base(returnTokenOnMatch)
    {
        Start = start;
        End = end;
        CountEndLength = countEndLength;
    }

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

        TToken token = GenerateToken(input.Slice(start: Start.Length, length: lessDistantLength));

        int length;
        if (CountEndLength) { length = Start.Length + lessDistantLength + endSpan[lessDistantIndex].Length; }
        else { length = Start.Length + lessDistantLength; }

        return new(
            isMatchFound: true,
            matchedTextLength: length,
            isEndOfFile: input.Length == length,
            generatedToken: token);
    }
}