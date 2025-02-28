
using System.Buffers;
using System.Collections;

namespace Lexer;

public abstract class Rule<TToken>
{
    public abstract bool Match(ReadOnlySpan<char> text, out int length, out TToken token);
}

public class SkipRule<TToken> : Rule<TToken>
{
    protected ReadOnlyMemory<string> ValuesToSkip { get; set; }

    public SkipRule(string[] valuesToSkip)
    {
        string[] values = valuesToSkip.ToArray();
        Array.Sort(values, (a, b) => a.Length - b.Length);

        if (values[0].Length == 0) { throw new Exception(); }

        ValuesToSkip = values.AsMemory();
    }

    public override bool Match(ReadOnlySpan<char> text, out int length, out TToken token)
    {
        ReadOnlySpan<string> values = ValuesToSkip.Span;
        length = 0;
        token = default!;

        for (int i = 0; i < values.Length; i++)
        {
            ReadOnlySpan<char> skip = values[i];
            int offset = skip.Length;
            if (offset > text.Length) { break; }

            if (skip.SequenceEqual(text[..offset]))
            {
                length += offset;
                i = 0;
            }

            while (skip.SequenceEqual(text.Slice(length, offset))) { length += offset; }
        }

        return length > 0;
    }
}