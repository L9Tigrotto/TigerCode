
using Lexer;

namespace ExtendedBackusNaurForm;

#region Patterns

public class AlternativePattern : IPattern<Token>
{
	public char[] ActivationSymbols => Utilities.AlternativeAS;
	public bool ProcessMatch(Context<Token> context) { return Utilities.AlternativePatternMatch(context); }
}

public class CommentPattern : IPattern<Token>
{
	public char[] ActivationSymbols => Utilities.CommentAS;
	public bool ProcessMatch(Context<Token> context) { return Utilities.CommentPatternMatch(context); }
}

public class DefinitionPattern : IPattern<Token>
{
	public char[] ActivationSymbols => Utilities.DefinitionAS;
	public bool ProcessMatch(Context<Token> context) { return Utilities.DefinitionMatch(context); }
}

public class ElementPattern : IPattern<Token>
{
	public char[] ActivationSymbols => Utilities.ElementAS;
	public bool ProcessMatch(Context<Token> context) { return Utilities.ElementMatch(context); }
}

public class WhiteSpacePattern : IPattern<Token>
{
	public char[] ActivationSymbols => Utilities.WhiteSpaceAS;
	public bool ProcessMatch(Context<Token> context) { return Utilities.WhiteSpaceMatch(context); }
}

#endregion

#region States

public class AlternativeState : IState<Token>
{
	public IState<Token>[] FollowingStates { get; set; } = [];
	public bool ProcessMatch(Context<Token> context) { return Utilities.AlternativeStateCheck(context); }
}

public class CommentState : IState<Token>
{
	public IState<Token>[] FollowingStates { get; set; } = [];
	public bool ProcessMatch(Context<Token> context) { return Utilities.CommentStateCheck(context); }
}

public class DefinitionState : IState<Token>
{
	public IState<Token>[] FollowingStates { get; set; } = [];
	public bool ProcessMatch(Context<Token> context) { return Utilities.DefinitionStateCheck(context); }
}

public class ElementState : IState<Token>
{
	public IState<Token>[] FollowingStates { get; set; } = [];
	public bool ProcessMatch(Context<Token> context) { return Utilities.ElementMatch(context); }
}

public class WhiteSpaceState : IState<Token>
{
	public IState<Token>[] FollowingStates { get; set; } = [];
	public bool ProcessMatch(Context<Token> context) { return Utilities.WhiteSpaceStateCheck(context); }
}

#endregion

#region Lexers

public class EBNFPatternLexer(FileInfo fileInfo, ErrorCollection errors, IPattern<Token>[] patterns) : PatternLexer<Token>(fileInfo, errors, patterns)
{
	protected override Token GenerateEmptyToken() { return new(); }
}

public class EBNFStateLexer(FileInfo fileInfo, ErrorCollection errors, IState<Token>[] states) : StateLexer<Token>(fileInfo, errors, states)
{
	protected override Token GenerateEmptyToken() { return new(); }
}

#endregion

public static class Manager
{
	public static IPattern<Token>[] CreatePatterns()
	{
		return
		[
			new WhiteSpacePattern(),
			new ElementPattern(),
			new CommentPattern(),
			new AlternativePattern(),
			new DefinitionPattern(),
		];
	}

	public static IState<Token>[] CreateStates()
	{
		WhiteSpaceState whiteSpace = new();
		ElementState element = new();
		CommentState comment = new();
		AlternativeState alternative = new();
		DefinitionState definition = new();

		whiteSpace.FollowingStates = [whiteSpace, element, comment, alternative, definition];
		element.FollowingStates = [whiteSpace, element, comment, alternative, definition];
		comment.FollowingStates = [whiteSpace, element, comment, alternative];
		alternative.FollowingStates = [whiteSpace, element, comment];
		definition.FollowingStates = [whiteSpace, element, comment];

		return
		[
			whiteSpace,
			element,
			comment,
			alternative,
			definition,
		];
	}
}
