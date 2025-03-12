
using Lexer;

namespace Backus_Naur_Form.Patterns;

public class SingleLineComment : IPattern<EBNFToken>
{
    private static readonly char[] _activationSymbols = ['/'];

    public char[] ActivationSymbols => _activationSymbols;

    public void ConfirmMatch(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;

        // input[0] is already one of the activation symbols
        if (input.Length < 2 || input[1] != '/') { return; }

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
}

