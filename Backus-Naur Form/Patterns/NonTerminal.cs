
using Lexer;

namespace Backus_Naur_Form.Patterns;

public class NonTerminal : IPattern<EBNFToken>
{
    private static readonly char[] _activationSymbols = ['<'];

    public char[] ActivationSymbols => _activationSymbols;

    public void ConfirmMatch(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;

        // skip the "<"
        input = input[1..];
        int index = input.IndexOf('>');

        if (index is -1) { return; }

        details.Token.Type = EBNFTokenType.NonTerminalSymbol; 
        details.Token.Value = details.Input.Slice(start: 1, length: index);
        details.Input = details.Input[(index + 2)..];
        details.IsMatch = true;
    }
}
