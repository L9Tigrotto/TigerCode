
using Lexer;

namespace Backus_Naur_Form.Patterns;

public class MultiLineComment : IPattern<EBNFToken>
{
    private static readonly char[] _activationSymbols = ['/'];

    public char[] ActivationSymbols => _activationSymbols;

    public void ConfirmMatch(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;

        // input[0] is already one of the activation symbols
        if (input.Length < 4 || input[1] != '*') { return; }

        // skip the "/*"
        input = input[2..];

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
