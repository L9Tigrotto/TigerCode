
namespace Lexer;

/// <summary>
/// Abstract base class for lexer rules.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer rule.</typeparam>
public abstract class LexerRule<TToken>
{
    /// <summary>
    /// Attempts to match the input text and produce a token.
    /// </summary>
    /// <param name="text">The input text to match.</param>
    /// <param name="length">The length of the matched text.</param>
    /// <param name="token">The produced token if a match is found.</param>
    /// <returns>True if a match is found, otherwise false.</returns>
    public abstract bool Match(ReadOnlyMemory<char> text, out int length, out TToken token);
}

/// <summary>
/// A lexer rule that skips specified values in the input text.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer rule.</typeparam>
public class SkipRule<TToken> : LexerRule<TToken>
{
    /// <summary>
    /// Gets the values to skip in the input text.
    /// </summary>
    protected ReadOnlyMemory<string> ValuesToSkip { get; init; }

    /// <summary>
    /// Initializes a new instance of the SkipRule class with the specified values to skip.
    /// </summary>
    /// <param name="valuesToSkip">The values to skip in the input text.</param>
    /// <exception cref="Exception">Thrown if any value to skip is an empty string.</exception>
    public SkipRule(string[] valuesToSkip)
    {
        // Sort values by length to optimize matching
        string[] values = valuesToSkip.ToArray();
        Array.Sort(values, (a, b) => a.Length - b.Length);

        if (values[0].Length == 0) { throw new ArgumentException("Empty string detected."); }

        ValuesToSkip = values.AsMemory();
    }

    /// <summary>
    /// Attempts to match and skip the specified values in the input text.
    /// </summary>
    /// <param name="text">The input text to match.</param>
    /// <param name="length">The length of the matched text.</param>
    /// <param name="token">The produced token if a match is found.</param>
    /// <returns>True if a match is found and text is skipped, otherwise false.</returns>
    public override bool Match(ReadOnlyMemory<char> text, out int length, out TToken token)
    {
        ReadOnlySpan<string> values = ValuesToSkip.Span;
        ReadOnlySpan<char> span = text.Span;
        length = 0;
        token = default!;

        // Iterate over each value to skip and check for matches in the text
        for (int i = 0; i < values.Length; i++)
        {
            ReadOnlySpan<char> value = values[i];
            int offset = value.Length;

            // Skip values longer than the remaining text
            if (offset > text.Length) { break; }

            // Check for matches at the start of the text
            if (value.SequenceEqual(span.Slice(length, offset)))
            {
                length += offset;
                i = -1; // Reset index to check for additional matches
            }
            else { continue; }

            // Check for additional matches of the same value
            while (value.SequenceEqual(span.Slice(length, offset))) { length += offset; }
        }

        // Return true if any text was matched and skipped
        return length > 0;
    }
}

/// <summary>
/// A lexer rule that matches the end of the input text and produces a specified token.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer rule.</typeparam>
public class EndOfFileRule<TToken>(TToken token) : LexerRule<TToken>
{
    /// <summary>
    /// Gets the token produced when the end of the input text is matched.
    /// </summary>
    public TToken EndOfFIleToken { get; init; } = token;

    /// <summary>
    /// Attempts to match the end of the input text.
    /// </summary>
    /// <param name="text">The input text to match.</param>
    /// <param name="length">The length of the matched text (always 0 for end-of-file).</param>
    /// <param name="token">The produced token if a match is found.</param>
    /// <returns>True if the input text is empty (end-of-file), otherwise false.</returns>
    public override bool Match(ReadOnlyMemory<char> text, out int length, out TToken token)
    {
        length = 0; // No text is matched for end-of-file
        token = EndOfFIleToken!; // Produce the specified end-of-file token

        // Return true if the input text is empty, indicating end-of-file
        return text.Length == 0;
    }
}

/// <summary>
/// A lexer rule that matches an exact sequence of characters and produces a specified token.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer rule.</typeparam>
public class ExactMatchRule<TToken>(ReadOnlyMemory<char> toMatch, TToken token) : LexerRule<TToken>
{
    /// <summary>
    /// Gets the sequence of characters to match in the input text.
    /// </summary>
    protected ReadOnlyMemory<char> ToMatch { get; init; } = toMatch;

    /// <summary>
    /// Gets the token produced when the exact match is found.
    /// </summary>
    protected TToken Token { get; init; } = token;
    
    /// <summary>
    /// Attempts to match the exact sequence of characters in the input text.
    /// </summary>
    /// <param name="text">The input text to match.</param>
    /// <param name="length">The length of the matched text.</param>
    /// <param name="token">The produced token if a match is found.</param>
    /// <returns>True if the exact match is found, otherwise false.</returns>
    public override bool Match(ReadOnlyMemory<char> text, out int length, out TToken token)
    {
        ReadOnlySpan<char> span = text.Span;

        // Check if the input text starts with the exact sequence to match
        if (span.Length < ToMatch.Length || !span[..ToMatch.Length].SequenceEqual(ToMatch.Span))
        {
            length = 0;
            token = default!;
            return false;
        }

        length = ToMatch.Length;
        token = Token;
        return true;
    }
}