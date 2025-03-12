
using System.Buffers;
using Lexer;

namespace Backus_Naur_Form.Patterns;

public class WhiteSpacePattern : IPattern<EBNFToken>
{
	private static readonly char[] _activationSymbols = [' ', '\t', '\n', '\r'];

	public char[] ActivationSymbols => _activationSymbols;
	public SearchValues<char> SearchActivationSymbols { get; set; } = SearchValues.Create(_activationSymbols);
	public int LineCount { get; set; } = 1;

	public void ConfirmMatch(MatchDetails<EBNFToken> details)
	{
		ReadOnlySpan<char> input = details.Input.Span;

		// input[0] is already one of the activation symbols
		if (input[0] == '\n') { LineCount++; }

		int occurrences = 1;
		while (occurrences < input.Length && SearchActivationSymbols.Contains(input[occurrences]))
		{
			if (input[occurrences] == '\n') { LineCount++; }
			occurrences++;
		}

		details.Input = details.Input[occurrences..];
		details.IsMatch = true;
		details.Skip = true;
	}
}

