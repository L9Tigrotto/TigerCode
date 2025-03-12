
namespace Lexer;

/// <summary>
/// Contains information about the current match being processed.
/// </summary>
/// <typeparam name="T">The type of token being matched.</typeparam>
public class MatchDetails<T> where T : IToken
{
	/// <summary>
	/// Gets or sets the remaining input to be processed.
	/// </summary>
	public ReadOnlyMemory<char> Input { get; set; }

	/// <summary>
	/// Gets or sets the current token being matched. This token is reused multiple <see cref="IPattern{T}.ConfirmMatch(MatchDetails{T} details)"/>.
	/// </summary>
	public T Token { get; init; }

	/// <summary>
	/// Gets or sets a value indicating whether a match has been found.
	/// </summary>
	public bool IsMatch { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether to skip returning the current token.
	/// </summary>
	public bool Skip { get; set; }

	/// <summary>
	/// Initializes a new instance of the MatchDetails class with the specified input and token.
	/// </summary>
	/// <param name="input">The input to be processed.</param>
	/// <param name="token">The initial token, which will be reused.</param>
	public MatchDetails(ReadOnlyMemory<char> input, T token)
	{
		Input = input;
		Token = token;

		// Initialize match status properties.
		IsMatch = false;
		Skip = false;
	}

	/// <summary>
	/// Resets the match details to their initial state.
	/// </summary>
	public void Reset()
	{
		// Reset match status properties.
		IsMatch = false;
		Skip = false;

		// Reset the token to its initial state.
		Token.Reset();
	}
}

/// <summary>
/// Defines a pattern that can be applied to tokenize input.
/// </summary>
/// <typeparam name="T">The type of token to be produced, which must implement IToken.</typeparam>
public interface IPattern<T> where T : IToken
{
	/// <summary>
	/// Gets the activation symbols that trigger this pattern.
	/// </summary>
	public char[] ActivationSymbols { get; }

	/// <summary>
	/// Confirms whether the input matches this pattern and updates the match details accordingly.
	/// </summary>
	/// <param name="details">The match details to update.</param>
	/// <remarks>
	/// Guidelines for implementation:<br />
	/// - If there is a match and the token is set to be skipped, the function should check for consecutive matches.<br />
	/// - When there is a match, the function should resize the input based on the match length only.
	/// </remarks>
	void ConfirmMatch(MatchDetails<T> details);
}

