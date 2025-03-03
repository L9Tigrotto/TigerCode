
using Lexer;

namespace Backus_Naur_Form.Rules.Basics;

/// <summary>
/// A lexer rule that matches any of the specified exact sequences of characters and produces a single token.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer rule.</typeparam>
public class MatchExactSequencesRule<TToken> : LexerRule<TToken>
{
    /// <summary>
    /// Gets the sequences of characters to match in the input text.
    /// </summary>
    protected ReadOnlyMemory<string> Sequences { get; init; }

    /// <summary>
    /// Gets the token produced when any of the exact sequences are found.
    /// </summary>
    protected TToken Token { get; init; }

    /// <summary>
    /// Initializes a new instance of the MatchExactSequencesRule class with the specified sequences and token.
    /// </summary>
    /// <param name="sequences">The sequences of characters to match.</param>
    /// <param name="token">The token to produce when any of the sequences are found.</param>
    /// <param name="returnTokenOnMatch">True to return the token on match; false to skip the token.</param>
    /// <exception cref="ArgumentException">Thrown if any sequence in the sequences array is empty.</exception>
    public MatchExactSequencesRule(ReadOnlyMemory<string> sequences, TToken token, bool returnTokenOnMatch = true) : base(returnTokenOnMatch)
    {
        ReadOnlySpan<string> sequencesSpan = sequences.Span;
        for (int i = 0; i < sequencesSpan.Length; i++)
        {
            if (sequencesSpan[i].Length is 0) { throw new ArgumentException($"Sequences[{i}] is empty."); }
        }
        
        Sequences = sequences;
        Token = token;
    }

    /// <summary>
    /// Attempts to match the input text with any of the exact sequences of characters.
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
        ReadOnlySpan<string> sequences = Sequences.Span;
        ReadOnlySpan<char> inputSpan = input.Span;

        for (int i = 0; i < sequences.Length; i++)
        {
            ReadOnlySpan<char> sequence = sequences[i];
            if (sequence.Length > input.Length) { continue; }

            ReadOnlySpan<char> temp = inputSpan[..sequence.Length];
            if (temp.SequenceEqual(sequence))
            {
                return new(
                    isMatchFound: true,
                    matchedTextLength: sequence.Length,
                    isEndOfFile: input.Length == sequence.Length,
                    generatedToken: Token);
            }
        }

        return new(isMatchFound: false, matchedTextLength: 0, isEndOfFile: false, generatedToken: default!);
    }
}