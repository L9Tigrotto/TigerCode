
using Lexer;
using System.Buffers;

namespace Backus_Naur_Form.Patterns;

public class ElementPattern : IPattern<EBNFToken>
{
    private static readonly char[] _activationSymbols = ['<', '"', '(', ')', '[', ']', '{', '}'];
    private static readonly SearchValues<char> _searchInvalidSymbols = SearchValues.Create('"', '\n', '\\');

    public char[] ActivationSymbols => _activationSymbols;


    public void ConfirmMatch(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;

        switch (input[0])
        {
            case '<': NonTerminal(details); break;
            case '"': Terminal(details); break;

            case '(': GroupingStart(details); break;
            case ')': GroupingEnd(details); break;

            case '[': OptionalStart(details); break;
            case ']': OptionalEnd(details); break;

            case '{': RepetitionStart(details); break;
            case '}': RepetitionEnd(details); break;
        }
    }

    private static void NonTerminal(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;

        // skip the "<"
        input = input[1..];
        int index = input.IndexOf('>');

        if (index is -1) { return; }

        details.Token.Type = EBNFTokenType.NonTerminalElement;
        details.Token.Value = details.Input.Slice(start: 1, length: index);
        details.Input = details.Input[(index + 2)..];
        details.IsMatch = true;
    }

    private static void Terminal(MatchDetails<EBNFToken> details)
    {
        ReadOnlySpan<char> input = details.Input.Span;

        // skip the """
        input = input[1..];

        int length = 0;
        while (true)
        {
            int index = input.IndexOfAny(_searchInvalidSymbols);
            if (index is -1 || input[index] is '\n') { return; }

            length += index + 1;
            if (input[index] is '"' || index + 1 == input.Length) { break; }

            length++;
            input = input[(index + 2)..];
        }

        details.Token.Type = EBNFTokenType.TerminalElement;
        details.Token.Value = details.Input[1..length];
        details.Input = details.Input[(length + 2)..];
        details.IsMatch = true;
    }

    private static void GroupingStart(MatchDetails<EBNFToken> details)
    {
        details.Token.Type = EBNFTokenType.GroupingElementStart;
        details.Input = details.Input[1..];
        details.IsMatch = true;
    }

    private static void GroupingEnd(MatchDetails<EBNFToken> details)
    {
        details.Token.Type = EBNFTokenType.GroupingElementEnd;
        details.Input = details.Input[1..];
        details.IsMatch = true;
    }

    private static void OptionalStart(MatchDetails<EBNFToken> details)
    {
        details.Token.Type = EBNFTokenType.OptionalElementStart;
        details.Input = details.Input[1..];
        details.IsMatch = true;
    }

    private static void OptionalEnd(MatchDetails<EBNFToken> details)
    {
        details.Token.Type = EBNFTokenType.OptionalElementEnd;
        details.Input = details.Input[1..];
        details.IsMatch = true;
    }

    private static void RepetitionStart(MatchDetails<EBNFToken> details)
    {
        details.Token.Type = EBNFTokenType.RepetitionElementStart;
        details.Input = details.Input[1..];
        details.IsMatch = true;
    }

    private static void RepetitionEnd(MatchDetails<EBNFToken> details)
    {
        details.Token.Type = EBNFTokenType.RepetitionElementEnd;
        details.Input = details.Input[1..];
        details.IsMatch = true;
    }
}

