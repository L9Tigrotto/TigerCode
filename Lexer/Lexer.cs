
namespace Lexer;

/// <summary>
/// A generic lexer class that processes input text using specified rules.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer.</typeparam>
/// <remarks>
/// Initializes a new instance of the Lexer class with the specified input text and rules.
/// </remarks>
/// <param name="input">The input text to be processed.</param>
/// <param name="rules">The list of rules used to match tokens in the input text.</param>
public class Lexer<TToken>(ReadOnlyMemory<char> input, List<LexerRule<TToken>> rules)
{
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
    /// <param name="rules">The list of rules used to match tokens in the input text.</param>
    /// <returns>A new Lexer instance initialized with the file's content.</returns>
    public static Lexer<TToken> From(FileInfo fileInfo, List<LexerRule<TToken>> rules)
    {
        string content = File.ReadAllText(fileInfo.FullName);
        return new(content.AsMemory(), rules);
    }

    /// <summary>
    /// Adds a new rule to the list of rules.
    /// </summary>
    /// <param name="lexerRule">The rule to add.</param>
    public void AddRule(LexerRule<TToken> lexerRule) { Rules.Add(lexerRule); }

    /// <summary>
    /// Retrieves the next token from the input text.
    /// </summary>
    /// <param name="isEndOfFile">True if the end of the file has been reached; otherwise, false.</param>
    /// <returns>The next token produced by the lexer rules.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no token is matched.</exception>
    public TToken NextToken(out bool isEndOfFile)
    {
        ReadOnlyMemory<char> text = Input[Cursor..];

        // Attempt to match tokens using the rules
        for (int i = 0; i < Rules.Count; i++)
        {
            MatchResult<TToken> result = Rules[i].Match(text);
            if (!result.IsMatchFound) { continue; }

            Cursor += result.MatchedTextLength;

            if (!Rules[i].ReturnTokenOnMatch)
            {
                text = Input[Cursor..];
                i = -1;
                continue;
            }

            isEndOfFile = result.IsEndOfFile;
            return result.GeneratedToken;
        }

        // Throw an exception if no token is matched
        int textLength = Math.Min(text.Length, 20);
        ReadOnlySpan<char> textToPrint = text.Span[..textLength];
        throw new InvalidOperationException($"No rules matched the input text. '{textToPrint}'..");
    }
}

