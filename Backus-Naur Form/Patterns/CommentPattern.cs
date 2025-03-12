

using Lexer;

namespace Backus_Naur_Form.Patterns;

public class CommentPattern : IPattern<EBNFToken>
{
	private static readonly char[] _activationSymbols = ['/'];

	public char[] ActivationSymbols => _activationSymbols;

	public void ConfirmMatch(MatchDetails<EBNFToken> details)
	{
		if (details.Input.Length < 2) { return; }

		ReadOnlySpan<char> input = details.Input.Span;
		if (input[1] is '/') { SingleLineComment(details); }
		else { MultiLineComment(details); }
	}

	private static void SingleLineComment(MatchDetails<EBNFToken> details)
	{
		ReadOnlySpan<char> input = details.Input.Span;

		// skip the "//"
		input = input[2..];
		int index = input.IndexOf('\n');

		if (index is -1)
		{
			// the comment is at the end of the file and there is no line feed.
			details.Input = ReadOnlyMemory<char>.Empty;
			details.IsMatch = true;
			details.Skip = true;
		}

		// make the whitespace rule handle the '\r'
		if (index >= 1 && input[index - 1] == '\r') { index--; }

		details.Input = details.Input[(index + 2)..];
		details.IsMatch = true;
		details.Skip = true;
	}

	private static void MultiLineComment(MatchDetails<EBNFToken> details)
	{
		ReadOnlySpan<char> input = details.Input.Span;

		// input[0] is already one of the activation symbols
		if (input.Length < 4) { return; }

		// skip the "/*"
		input = input[2..];

		// TODO: known bug: '\n' might be skipped and line count on WhiteSpacePattern could be off
		int length = 2;
		while (true)
		{
			int index = input.IndexOf('/');
			if (index is -1) { return; }

			length += index + 1;
			if (index == 0) { input = input[1..]; continue; }
			if (input[index - 1] == '*') { break; }

			input = input[(index + 1)..];
		}

		details.Input = details.Input[length..];
		details.IsMatch = true;
		details.Skip = true;
	}
}

