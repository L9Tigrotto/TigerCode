
using System.Buffers;
using Lexer;

namespace Backus_Naur_Form.Patterns;

class Terminal : IPattern<EBNFToken>
{
    private static readonly char[] _activationSymbols = ['"'];

    public char[] ActivationSymbols => _activationSymbols;

    public SearchValues<char> SearchInvalidSymbols { get; set; } = SearchValues.Create('"', '\n', '\\');

    public void ConfirmMatch(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;

        // skip the """
        input = input[1..];

        int length = 0;
        while (true)
        {
            int index = input.IndexOfAny(SearchInvalidSymbols);
            if (index is -1 || input[index] is '\n') { return; }

            length += index + 1;
            if (input[index] is '"' || index + 1 == input.Length) { break; }

            length++;
            input = input[(index + 2)..];
        }

        details.Token.Type = EBNFTokenType.TerminalSymbol;
        details.Token.Value = details.Input.Slice(start: 1, length - 1);
        details.Input = details.Input[(length + 2)..];
        details.IsMatch = true;
    }
}
