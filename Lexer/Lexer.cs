
namespace Lexer;

/// <summary>
/// A generic lexer class that processes input text using specified rules.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer.</typeparam>
public class Lexer<TToken>(ReadOnlyMemory<char> input, LexerRule<TToken> skip, LexerRule<TToken> endOfFile, List<LexerRule<TToken>> rules)
{
    /// <summary>
    /// Gets the rule used to skip parts of the input text.
    /// </summary>
    protected LexerRule<TToken> Skip { get; init; } = skip;

    /// <summary>
    /// Gets the rule used to detect the end of the input text.
    /// </summary>
    protected LexerRule<TToken> EndOfFile { get; init; } = endOfFile;

    /// <summary>
    /// Gets the list of rules used to match tokens in the input text.
    /// </summary>
    protected List<LexerRule<TToken>> Rules { get; init; } = rules;

    /// <summary>
    /// Gets or sets the input text to be processed.
    /// </summary>
    protected ReadOnlyMemory<char> Input { get; set; } = input;

    /// <summary>
    /// Gets or sets the current position in the input text.
    /// </summary>
    protected int Cursor { get; set; } = 0;

    /// <summary>
    /// Creates a new Lexer instance from a file.
    /// </summary>
    /// <param name="fileInfo">The file to read the input text from.</param>
    /// <param name="skip">The rule used to skip parts of the input text.</param>
    /// <param name="endOfFile">The rule used to detect the end of the input text.</param>
    /// <param name="rules">The list of rules used to match tokens in the input text.</param>
    /// <returns>A new Lexer instance initialized with the file's content.</returns>
    public static Lexer<TToken> From(FileInfo fileInfo, LexerRule<TToken> skip, LexerRule<TToken> endOfFile, List<LexerRule<TToken>> rules)
    {
        string content = File.ReadAllText(fileInfo.FullName);
        return new(content.AsMemory(), skip, endOfFile, rules);
    }

    /// <summary>
    /// Adds a new rule to the list of rules.
    /// </summary>
    /// <param name="lexerRule">The rule to add.</param>
    public void AddRule(LexerRule<TToken> lexerRule) { Rules.Add(lexerRule); }

    /// <summary>
    /// Retrieves the next token from the input text.
    /// </summary>
    /// <returns>The next token produced by the lexer rules.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no token is matched.</exception>
    public TToken NextToken()
    {
        ReadOnlyMemory<char> text = Input[Cursor..];

        // Check for end-of-file condition
        bool match = EndOfFile.Match(text, out _, out TToken token);
        if (match) { return token; }

        // Attempt to skip parts of the input text
        match = Skip.Match(text, out int length, out _);
        if (match)
        {
            text = text[length..];
            Cursor += length;
        }

        // Check again for end-of-file condition after skipping
        match = EndOfFile.Match(text, out _, out token);
        if (match) { return token; }

        // Attempt to match tokens using the rules
        for (int i = 0; i < Rules.Count; i++)
        {
            match = Rules[i].Match(text, out length, out token);

            if (!match) { continue; }

            Cursor += length;
            return token;
        }

        // Throw an exception if no token is matched
        length = Math.Min(text.Length, 20);
        ReadOnlySpan<char> textToPrint = text.Span[..length];
        throw new InvalidOperationException($"No rules matched the input text. '{textToPrint}'..");
    }
}

