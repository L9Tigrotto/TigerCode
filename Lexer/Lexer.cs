
namespace Lexer;

/// <summary>
/// A generic lexer class that processes input text using specified rules.
/// </summary>
/// <typeparam name="TToken">The type of token produced by the lexer.</typeparam>
public class Lexer<TToken>
{
    /// <summary>
    /// Gets or sets the input text to be processed.
    /// </summary>
    protected ReadOnlyMemory<char> Input { get; set; }

    /// <summary>
    /// Gets all the rules used by the lexer to match tokens in the input text.
    /// </summary>
    protected LexerRule<TToken>[] AllRules { get; init; }

    /// <summary>
    /// Gets or sets the rules that are expected to follow the last matched rule.
    /// These rules are prioritized for matching the next token.
    /// </summary>
    protected LexerRule<TToken>[] ExpectedFollowUpRules { get; set; }

    /// <summary>
    /// Gets or sets the current position in the input text.
    /// </summary>
    protected int Cursor { get; set; } = 0;

    /// <summary>
    /// Initializes a new instance of the Lexer class with the specified input text and rules.
    /// </summary>
    /// <param name="input">The input text to be processed.</param>
    /// <param name="allRules">The list of rules used to match tokens in the input text.</param>
    public Lexer(ReadOnlyMemory<char> input, LexerRule<TToken>[] allRules)
    {
        Input = input;
        AllRules = allRules;
        ExpectedFollowUpRules = [];
        Cursor = 0;
    }

    /// <summary>
    /// Creates a new Lexer instance from a file.
    /// </summary>
    /// <param name="fileInfo">The file to read the input text from.</param>
    /// <param name="rules">The list of rules used to match tokens in the input text.</param>
    /// <returns>A new Lexer instance initialized with the file's content.</returns>
    public static Lexer<TToken> From(FileInfo fileInfo, LexerRule<TToken>[] rules)
    {
        string content = File.ReadAllText(fileInfo.FullName);
        return new(content.AsMemory(), rules);
    }

    /// <summary>
    /// Updates the lexer's state after a successful match.
    /// </summary>
    /// <param name="input">The input text being processed.</param>
    /// <param name="result">The result of the match operation.</param>
    /// <param name="rule">The rule that matched the input text.</param>
    private void UpdateStateForMatch(ref ReadOnlyMemory<char> input,  MatchResult<TToken> result, LexerRule<TToken> rule)
    {
        input = input[result.MatchedTextLength..];
        ExpectedFollowUpRules = rule.SubsequentRules;
        Cursor += result.MatchedTextLength;
    }

    /// <summary>
    /// Iterates over the ExpectedFollowUpRules to find a match in the input text.
    /// </summary>
    /// <param name="input">The input text to match.</param>
    /// <param name="isTokenGenerated">True if a token was generated; otherwise, false.</param>
    /// <param name="isEndOfFile">True if the end of the file has been reached; otherwise, false.</param>
    /// <returns>The generated token if a match is found; otherwise, default.</returns>
    public TToken IterateFollowUpRules(ref ReadOnlyMemory<char> input, out bool isTokenGenerated, out bool isEndOfFile)
    {
        for (int i = 0; i < ExpectedFollowUpRules.Length; i++)
        {
            LexerRule<TToken> rule = ExpectedFollowUpRules[i];
            MatchResult<TToken> result = rule.Match(input);

            // Check if the end of the file is reached
            if (result.IsEndOfFile)
            {
                if (result.IsMatchFound) { UpdateStateForMatch(ref input, result, rule); }

                isTokenGenerated = true;
                isEndOfFile = true;
                return result.GeneratedToken;
            }

            if (result.IsMatchFound)
            {
                UpdateStateForMatch(ref input, result, rule);

                if (!rule.ReturnTokenOnMatch)
                {
                    i = -1; // Restart the loop to re-evaluate the rules
                    continue;
                }

                isTokenGenerated = true;
                isEndOfFile = false;
                return result.GeneratedToken;
            }
        }

        isTokenGenerated = false;
        isEndOfFile = false;
        return default!;
    }

    /// <summary>
    /// Iterates over all rules to find a match in the input text.
    /// </summary>
    /// <param name="input">The input text to match.</param>
    /// <param name="isTokenGenerated">True if a token was generated; otherwise, false.</param>
    /// <param name="reiterateFollowUpRules">True if the follow-up rules should be re-evaluated; otherwise, false.</param>
    /// <param name="isEndOfFile">True if the end of the file has been reached; otherwise, false.</param>
    /// <returns>The generated token if a match is found; otherwise, default.</returns>
    public TToken IterateAllRules(ref ReadOnlyMemory<char> input, out bool isTokenGenerated, out bool reiterateFollowUpRules, out bool isEndOfFile)
    {
        for (int i = 0; i < AllRules.Length; i++)
        {
            LexerRule<TToken> rule = AllRules[i];
            MatchResult<TToken> result = rule.Match(input);

            // Check if the end of the file is reached
            if (result.IsEndOfFile)
            {
                if (result.IsMatchFound) { UpdateStateForMatch(ref input, result, rule); }

                isTokenGenerated = true;
                reiterateFollowUpRules = false;
                isEndOfFile = true;
                return result.GeneratedToken;
            }

            if (result.IsMatchFound)
            {
                UpdateStateForMatch(ref input, result, rule);

                isTokenGenerated = true;
                reiterateFollowUpRules = !rule.ReturnTokenOnMatch;
                isEndOfFile = false;
                return result.GeneratedToken;
            }
        }

        isTokenGenerated = false;
        reiterateFollowUpRules = false;
        isEndOfFile = false;
        return default!;
    }

    /// <summary>
    /// Retrieves the next token from the input text.
    /// </summary>
    /// <param name="isEndOfFile">True if the end of the file has been reached; otherwise, false.</param>
    /// <returns>The next token produced by the lexer rules.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no token is matched.</exception>
    public TToken NextToken(out bool isEndOfFile)
    {
        ReadOnlyMemory<char> input = Input[Cursor..];

        while (true)
        {
            // First, try to match using the ExpectedFollowUpRules
            TToken token = IterateFollowUpRules(ref input, out bool isTokenGenerated, out isEndOfFile);
            if (isTokenGenerated) { return token; }

            // If no match, try to match using all rules
            token = IterateAllRules(ref input, out isTokenGenerated, out bool reiterateFollowUpRules, out isEndOfFile);
            if (reiterateFollowUpRules) { continue; }
            if (isTokenGenerated) { return token; }

            // Throw an exception if no token is matched
            int textLength = Math.Min(input.Length, 20);
            ReadOnlySpan<char> textToPrint = input.Span[..textLength];
            throw new InvalidOperationException($"No rules matched the input text. '{textToPrint}'..");
        }
    }
}

