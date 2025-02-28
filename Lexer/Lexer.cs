
namespace Lexer;

public class Lexer<TToken>(ReadOnlyMemory<char> input, Rule<TToken> skip, List<Rule<TToken>> rules)
{
    private Rule<TToken> Skip { get; init; } = skip;
    private List<Rule<TToken>> Rules { get; init; } = rules;

    protected ReadOnlyMemory<char> Input { get; set; } = input;
    protected int Cursor { get; set; } = 0;

    public static Lexer<TToken> From(FileInfo fileInfo, Rule<TToken> skip, List<Rule<TToken>> rules)
    {
        string content = File.ReadAllText(fileInfo.FullName);
        return new(content.AsMemory(), skip, rules);
    }

    public void AddRule(Rule<TToken> rule) { Rules.Add(rule); }

    public TToken NextToken()
    {
        ReadOnlySpan<char> text = Input.Span[Cursor..];
        bool match = Skip.Match(text, out int length, out _);

        if (match)
        {
            text = text[length..];
            Cursor += length;
        }

        for (int i = 0; i < Rules.Count; i++)
        {
            match = Rules[i].Match(text, out length, out TToken token);

            if (!match) { continue; }

            Cursor += length;
            return token;
        }

        throw new Exception();
    }
}

