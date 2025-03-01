
using Lexer;
using Lexer.DefaultRules;

namespace Backus_Naur_Form.LexerRules;

internal class WhiteSpaceRule : MatchExactSequencesRule<Token>
{
    public int CurrentLine { get; protected set; } = 1;

    private static readonly string[] WhiteSpaceSequences = [" ", "\t"];

    private static readonly Token WhiteSpaceToken = new(TokenType.WhiteSpace, WhiteSpaceSequences[0].AsMemory());

    public override MatchResult<Token> Match(ReadOnlyMemory<char> input)
    {
        MatchResult<Token> result = base.Match(input);
        if (result.IsMatchFound) { CurrentLine++; }
        return result;
    }

#if DEBUG
    public WhiteSpaceRule() : base(WhiteSpaceSequences.AsMemory(), WhiteSpaceToken, returnTokenOnMatch: true) { }

#else
    public WhiteSpaceRule() : base(WhiteSpaceSequences.AsMemory(), WhiteSpaceToken, returnTokenOnMatch: false) { }
#endif
}