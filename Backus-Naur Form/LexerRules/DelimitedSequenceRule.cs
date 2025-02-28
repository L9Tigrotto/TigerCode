
using System.Buffers;
using Lexer;

namespace Backus_Naur_Form.LexerRules;

/// <summary>
/// A lexer rule that matches a sequence of characters delimited by a start and end character,
/// excluding any characters specified as not allowed.
/// </summary>
public class DelimitedSequenceRule : LexerRule<Token>
{
    /// <summary>
    /// Gets the start character of the delimited sequence.
    /// </summary>
    protected char StartChar { get; init; }

    /// <summary>
    /// Gets the end character of the delimited sequence.
    /// </summary>
    protected char EndChar { get; init; }

    /// <summary>
    /// Gets the type of token produced by this rule.
    /// </summary>
    protected TokenType TokenType { get; init; }

    /// <summary>
    /// Gets the set of characters that are not allowed within the delimited sequence.
    /// </summary>
    protected SearchValues<char> NotAllowedChars { get; init; }

    /// <summary>
    /// Initializes a new instance of the DelimitedSequenceRule class.
    /// </summary>
    /// <param name="startChar">The start character of the delimited sequence.</param>
    /// <param name="endChar">The end character of the delimited sequence.</param>
    /// <param name="notAllowedChars">The characters not allowed within the sequence.</param>
    /// <param name="tokenType">The type of token produced by this rule.</param>
    public DelimitedSequenceRule(char startChar, char endChar, ReadOnlySpan<char> notAllowedChars, TokenType tokenType)
    {
        StartChar = startChar;
        EndChar = endChar;
        TokenType = tokenType;
        NotAllowedChars = SearchValues.Create(notAllowedChars);
    }

    /// <summary>
    /// Attempts to match a delimited sequence in the input text.
    /// </summary>
    /// <param name="text">The input text to match.</param>
    /// <param name="length">The length of the matched text.</param>
    /// <param name="token">The produced token if a match is found.</param>
    /// <returns>True if a match is found, otherwise false.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the end character is not found or if an invalid character is encountered.</exception>
    public override bool Match(ReadOnlyMemory<char> text, out int length, out Token token)
    {
        ReadOnlySpan<char> span = text.Span;

        // Check if the text starts with the start character
        if (span[0] != StartChar)
        {
            length = 0;
            token = null!;
            return false;
        }

        // Move past the start character
        span = span[1..];
        int index = span.IndexOfAny(NotAllowedChars);

        // Ensure the end character is found
        if (index is -1) { throw new InvalidOperationException($"Sequence end char '{EndChar}' not found."); }

        char invalidChar = span[index];
        if (invalidChar != EndChar)
        {
            throw new InvalidOperationException(
                $"Invalid sequence: {span[..index]}({invalidChar})?.");
        }

        // Calculate the length of the matched sequence
        length = index + 2; // Include start and end characters
        token = new(TokenType, text.Slice(start: 1, length: index));
        return true;
    }
}

/// <summary>
/// A lexer rule that matches a sequence of characters starting with a specific text and ending with a specific character,
/// excluding any characters specified as not allowed.
/// </summary>
public class DelimitedSequenceWithSequencesRule : LexerRule<Token>
{
    /// <summary>
    /// Gets the start text of the delimited sequence.
    /// </summary>
    protected ReadOnlyMemory<char> StartText { get; init; }

    /// <summary>
    /// Gets the end character of the delimited sequence.
    /// </summary>
    protected char EndChar { get; init; }

    /// <summary>
    /// Gets the type of token produced by this rule.
    /// </summary>
    protected TokenType TokenType { get; init; }

    /// <summary>
    /// Gets the set of characters that are not allowed within the delimited sequence.
    /// </summary>
    protected SearchValues<char> NotAllowedChars { get; init; }

    /// <summary>
    /// Initializes a new instance of the DelimitedSequenceWithSequencesRule class.
    /// </summary>
    /// <param name="startText">The start text of the delimited sequence.</param>
    /// <param name="endChar">The end character of the delimited sequence.</param>
    /// <param name="notAllowedChars">The characters not allowed within the sequence.</param>
    /// <param name="tokenType">The type of token produced by this rule.</param>
    public DelimitedSequenceWithSequencesRule(ReadOnlyMemory<char> startText, char endChar, ReadOnlySpan<char> notAllowedChars, TokenType tokenType)
    {
        StartText = startText;
        EndChar = endChar;
        TokenType = tokenType;
        NotAllowedChars = SearchValues.Create(notAllowedChars);
    }

    /// <summary>
    /// Attempts to match a delimited sequence starting with specific text in the input text.
    /// </summary>
    /// <param name="text">The input text to match.</param>
    /// <param name="length">The length of the matched text.</param>
    /// <param name="token">The produced token if a match is found.</param>
    /// <returns>True if a match is found, otherwise false.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the end character is not found or if an invalid character is encountered.</exception>
    public override bool Match(ReadOnlyMemory<char> text, out int length, out Token token)
    {
        ReadOnlySpan<char> span = text.Span;

        // Check if the text starts with the start text
        if (span.Length < StartText.Length || !span[..StartText.Length].SequenceEqual(StartText.Span))
        {
            length = 0;
            token = null!;
            return false;
        }

        // Move past the start text
        span = span[StartText.Length..];
        int index = span.IndexOfAny(NotAllowedChars);

        // Ensure the end character is found
        if (index is -1) { throw new InvalidOperationException($"Sequence end char '{EndChar}' not found."); }

        char invalidChar = span[index];
        if (invalidChar != EndChar)
        {
            throw new InvalidOperationException(
                $"Invalid sequence: {span[..index]}({invalidChar})?.");
        }

        // Calculate the length of the matched sequence
        length = StartText.Length + index + 1;
        token = new(TokenType, text.Slice(start: StartText.Length, length: index - StartText.Length + 1));
        return true;
    }
}