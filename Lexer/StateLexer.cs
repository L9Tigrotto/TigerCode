
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Lexer;

public interface IState<T>
{
	public IState<T>[] FollowingStates { get; }

	bool ProcessMatch(Context<T> context);
}

public abstract class StateLexer<T>(FileInfo file, ErrorCollection errors, IState<T>[] initialStates) : Lexer<T>(file, errors)
{
	protected IState<T>[] States { get; set; } = initialStates;

	public override bool NextToken([NotNullWhen(true)] out T? token)
	{ 
		START:
		if (Context.Input.IsEmpty)
		{
			token = default;
			return false;
		}

		ReadOnlySpan<char> inputSpan = Context.Input.Span;

		Context.ResetFlags();

		foreach (IState<T> state in States)
		{
			if (state.ProcessMatch(Context))
			{
				States = state.FollowingStates;
				break;
			}
		}

		// If no state matches the input, throw an exception.
		if (Context.Matched)
		{
			if (Context.SkipReturn) { goto START; }

			Debug.Assert(Context.Token is not null);
			token = Context.Token;
			Context.Token = GenerateEmptyToken();
			return true;
		}
		else
		{
			int minLength = Math.Min(Context.Input.Length, 20);
			ReadOnlySpan<char> preview = inputSpan[..minLength];

			Context.Errors.Add(Context.File, LineNumber,
				minLength < 20
					? $"No state matched the input '{preview}'."
					: $"No state matched the input '{preview}...'.");

			token = default;
			return false;
		}
	}
}

