
using Lexer;

namespace Backus_Naur_Form.Patterns;

public class DefinitionPattern : IPattern<EBNFToken>
{
    private static readonly char[] _activationSymbols = [':', ';'];

    private const string _definitionStart = "::=";

    public char[] ActivationSymbols => _activationSymbols;

    public void ConfirmMatch(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;
        if (input[1] == _activationSymbols[0]) { DefinitionStart(details); }
        else { DefinitionEnd(details); }
    }


    private static void DefinitionStart(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;

        if (input.Length < _definitionStart.Length) { return; }

        input = input[.._definitionStart.Length];
        if (input is not _definitionStart) { return; }

        details.Token.Type = EBNFTokenType.DefinitionStart;
        details.Token.Value = details.Input[..3];
        details.Input = details.Input[_definitionStart.Length..];
        details.IsMatch = true;
    }

    private static void DefinitionEnd(MatchDetails<EBNFToken> details)
    {
        details.Token.Type = EBNFTokenType.DefinitionEnd;
        details.Input = details.Input[1..];
        details.IsMatch = true;
    }
}

