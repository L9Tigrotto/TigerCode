
using Lexer;

namespace Backus_Naur_Form.Patterns;

public class StartDefinitionSymbol : IPattern<EBNFToken>
{
    private static readonly char[] _activationSymbols = [':'];
    private const string _definitionSymbol = "::=";

    public char[] ActivationSymbols => _activationSymbols;

    public void ConfirmMatch(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;

        if (input.Length < _definitionSymbol.Length) { return; }

        input = input[.._definitionSymbol.Length];
        if (input is not _definitionSymbol) { return; }

        details.Token.Type = EBNFTokenType.DefinitionStartSymbol;
        details.Token.Value = details.Input[..3];
        details.Input = details.Input[_definitionSymbol.Length..];
        details.IsMatch = true;
    }
}

