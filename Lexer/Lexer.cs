
using System.Buffers;
using System.Runtime.InteropServices;

namespace Lexer;

/// <summary>
/// Defines a token that can be reset to its initial state.
/// </summary>
public interface IToken
{
    /// <summary>
    /// Resets the token to its initial state.
    /// </summary>
    public void Reset();
}

/// <summary>
/// Represents a generic lexer for tokenizing input based on a set of patterns.
/// </summary>
/// <typeparam name="T">The type of token to be produced, which must implement IToken.</typeparam>
public class Lexer<T> where T : IToken
{
    /// <summary>
    /// Gets or sets the input to be tokenized.
    /// </summary>
    protected ReadOnlyMemory<char> Input { get; set; }

    /// <summary>
    /// Gets or sets the current token being processed.
    /// </summary>
    protected T Token { get; set; }

    /// <summary>
    /// Gets or sets the activation symbols used to identify potential matches.
    /// </summary>
    protected SearchValues<char> ActivationSymbols { get; set; }

    /// <summary>
    /// Gets or sets a dictionary mapping activation symbols to their corresponding patterns.
    /// </summary>
    protected Dictionary<char, LinkedList<IPattern<T>>> PossibleMatches { get; set; }

    /// <summary>
    /// Initializes a new instance of the Lexer class with the specified input, patterns, and token.
    /// </summary>
    /// <param name="input">The input to be tokenized.</param>
    /// <param name="patterns">The patterns to apply during tokenization.</param>
    /// <param name="token">The initial token.</param>
    public Lexer(ReadOnlyMemory<char> input, IPattern<T>[] patterns, T token)
    {
        Input = input;
        Token = token;

        List<char> chars = [];
        PossibleMatches = [];

        // Iterate over each pattern and collect activation symbols.
        foreach (IPattern<T> rule in patterns)
        {
            foreach (char c in rule.ActivationSymbols)
            {
                // If the activation symbol is not already in the dictionary, add it with a new list.
                if (!PossibleMatches.TryGetValue(c, out LinkedList<IPattern<T>>? list))
                {
                    list = [];
                    PossibleMatches[c] = list;
                }

                // Add the pattern to the list of patterns for this activation symbol.
                list.AddLast(rule);
                chars.Add(c);
            }
        }

        // Create a SearchValues object from the collected activation symbols.
        Span<char> charsSpan = CollectionsMarshal.AsSpan(chars);
        ActivationSymbols = SearchValues.Create(charsSpan);
    }

    /// <summary>
    /// Tokenizes the input and yields the resulting tokens.
    /// </summary>
    /// <returns>An enumerable collection of tokens.</returns>
    public IEnumerable<T> Tokenize()
    {
        MatchDetails<T> matchDetails = new(Input, Token);

        // Continue processing until the input is fully tokenized.
        while (!matchDetails.Input.IsEmpty)
        {
            matchDetails.Reset();

            ReadOnlySpan<char> inputSpan = matchDetails.Input.Span;
            char activationSymbol = inputSpan[0];

            // Check if there are any patterns for the current activation symbol.
            if (!PossibleMatches.TryGetValue(activationSymbol, out LinkedList<IPattern<T>>? patterns))
            {
                throw new InvalidOperationException($"No rules defined for activation symbol '{activationSymbol}'. Please check your lexical rules.");
            }

            // Attempt to match the input against each pattern.
            foreach (IPattern<T> pattern in patterns)
            {
                pattern.ConfirmMatch(matchDetails);
                if (matchDetails.IsMatch) { break; }
            }

            // If no pattern matches the input, throw an exception.
            if (!matchDetails.IsMatch)
            {
                ReadOnlySpan<char> preview = matchDetails.Input.Span[..Math.Min(matchDetails.Input.Length, 20)];
                throw new InvalidOperationException($"No rule matched the input '{preview}'... Please review the input and rules.");
            }

            // Yield the matched token if it is not marked to be skipped.
            if (!matchDetails.Skip) { yield return matchDetails.Token; }
        }
    }
}

