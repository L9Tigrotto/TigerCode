
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Lexer;

public interface IPattern<T>
{
	public char[] ActivationSymbols { get; }

	bool ProcessMatch(Context<T> context);
}

public abstract class PatternLexer<T> : Lexer<T>
{
	protected SearchValues<char> ActivationSymbols { get; init; }
	protected Dictionary<char, IPattern<T>[]> PossibleMatches { get; init; }

	protected PatternLexer(FileInfo fileInfo, ErrorCollection errors, IPattern<T>[] patterns) : base(fileInfo, errors)
	{
		List<char> chars = [];
		PossibleMatches = [];

		// Iterate over each pattern and collect activation symbols and (activation symbol, pattern[]).
		Dictionary<char, List<IPattern<T>>> possibleMatches = [];

		foreach (IPattern<T> pattern in patterns)
		{
			foreach (char c in pattern.ActivationSymbols)
			{
				// If the activation symbol is not already in the dictionary, add it with a new list.
				if (!possibleMatches.TryGetValue(c, out List<IPattern<T>>? list))
				{
					list = [];
					possibleMatches[c] = list;
				}

				// Add the pattern to the list of patterns for this activation symbol.
				list.Add(pattern);
				chars.Add(c);
			}
		}

		// Transform the dictionary of lists into a dictionary of arrays.
		foreach (KeyValuePair<char, List<IPattern<T>>> kvp in possibleMatches)
		{
			PossibleMatches[kvp.Key] = [.. kvp.Value];
		}

		// Create a SearchValues object from the collected activation symbols.
		Span<char> charsSpan = CollectionsMarshal.AsSpan(chars);
		ActivationSymbols = SearchValues.Create(charsSpan);
	}

	public override bool NextToken([NotNullWhen(true)] out T? token)
	{
START:
		if (Context.Input.IsEmpty)
		{
			token = default;
			return false;
		}

		ReadOnlySpan<char> inputSpan = Context.Input.Span;
		char activationSymbol = inputSpan[0];

		// Check if there are any patterns for the current activation symbol.
		if (!PossibleMatches.TryGetValue(activationSymbol, out IPattern<T>[]? patterns))
		{
			int minLength = Math.Min(Context.Input.Length, 20);
			ReadOnlySpan<char> preview = inputSpan[1..minLength];

			Context.Errors.Add(Context.File, LineNumber,
				minLength < 20
					? $"No patterns activated with '[{activationSymbol}]{preview}'."
					: $"No patterns activated with '[{activationSymbol}]{preview}...'.");

			token = default;
			return false;
		}

		Context.ResetFlags();

		// Attempt to match the input against each pattern.
		foreach (IPattern<T> pattern in patterns)
		{
			if (pattern.ProcessMatch(Context)) { break; }
		}

		// If no pattern matches the input, throw an exception.
		if (Context.Matched)
		{
			if (Context.SkipReturn) { goto START; }

			Debug.Assert(Context.Token is not null);
			token = Context.Token;
			Context.Token = GenerateEmptyToken();
			return true;
		}
		else
		{
			int minLength = Math.Min(Context.Input.Length, 20);
			ReadOnlySpan<char> preview = inputSpan[1..minLength];

			Context.Errors.Add(Context.File, LineNumber,
				minLength < 20
					? $"No pattern matched the input '[{activationSymbol}]{preview}'."
					: $"No pattern matched the input '[{activationSymbol}]{preview}...'.");

			token = default;
			return false;
		}
	}
}
