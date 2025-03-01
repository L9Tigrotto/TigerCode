
using Lexer;
using Lexer.DefaultRules;

namespace Backus_Naur_Form.LexerRules;

internal class NewLineRule : MatchExactSequencesRule<Token>
{
    public int CurrentLine { get; protected set; } = 1;

    private static readonly string[] NewLineSequences = ["\r\n", "\n"];

    private static readonly Token NewLineToken = new(TokenType.NewLine, @"\r\n".AsMemory());

    public override MatchResult<Token> Match(ReadOnlyMemory<char> input)
    {
        MatchResult<Token> result = base.Match(input);
        if (result.IsMatchFound) { CurrentLine++; }
        return result;
    }

#if DEBUG
    public NewLineRule() : base(NewLineSequences.AsMemory(), NewLineToken, returnTokenOnMatch: true) { }

#else
    public NewLineRule() : base(NewLineSequences.AsMemory(), NewLineToken, returnTokenOnMatch: false) { }
#endif
}

