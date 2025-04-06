
using Lexer;
using System.Buffers;
using System.Diagnostics;

namespace ExtendedBackusNaurForm;

internal static class Utilities
{
	#region ActivationSymbols

	public static readonly char[] AlternativeAS = ['|'];
	public static readonly char[] CommentAS = ['/'];
	public static readonly char[] DefinitionAS = [':', ';'];
	public static readonly char[] ElementAS = ['<', '"', '(', ')', '[', ']', '{', '}'];
	public static readonly char[] WhiteSpaceAS = [' ', '\t', '\n', '\r'];

	#endregion

	#region Stoppers

	public static readonly SearchValues<char> MultilineCommentStoppers = SearchValues.Create('/', '\\', '\n');

	/// <summary>
	/// array of chars that are not allowed in non-terminal elements. also cannot start with a digit.
	/// </summary>
	public static readonly char[] _invalidNonTerminalChars =
	[
		' ',  // Space
		'!',  // Exclamation mark
		'@',  // At symbol
		'#',  // Hash
		'$',  // Dollar sign
		'%',  // Percent
		'^',  // Caret
		'&',  // Ampersand
		'*',  // Asterisk
		'(',  // Left parenthesis
		')',  // Right parenthesis
		'+',  // Plus
		'=',  // Equals
		'[',  // Left bracket
		']',  // Right bracket
		'{',  // Left curly brace
		'}',  // Right curly brace
		';',  // Semicolon
		':',  // Colon
		'\'', // Single quote
		'"',  // Double quote
		'<',  // Less than
		'>',  // Greater than
		',',  // Comma
		'.',  // Period
		'/',  // Forward slash
		'\\', // Backslash
		'|',  // Pipe
		'?',  // Question mark
		'`',  // Backtick
		'~'   // Tilde
	];

	public static readonly SearchValues<char> NonTerminalStoppers = SearchValues.Create(_invalidNonTerminalChars);
	public static readonly SearchValues<char> TerminalStoppers = SearchValues.Create('"', '\n', '\\');
	public static readonly SearchValues<char> WhiteSpaceStoppers = SearchValues.Create(WhiteSpaceAS);

	#endregion

	#region CommonFunctions

	public static bool AlternativePatternMatch(Context<Token> context)
	{
		Token token = context.Token;

		token.Type = TokenType.Alternative;
		token.Value = context.Advance(amount: 1);
		context.Matched = true;

		return true;
	}

	public static bool AlternativeStateCheck(Context<Token> context)
	{
		ReadOnlySpan<char> input = context.Input.Span;
		return input[0] is '|' && AlternativePatternMatch(context);
	}

	public static bool CommentPatternMatch(Context<Token> context)
	{
		if (context.Input.Length < 2) { return false; }

		ReadOnlySpan<char> input = context.Input.Span;
		char secondChar = input[1];

		// skip the activation symbol
		input = input[2..];

		if (secondChar is '/') { return SingleLineCommentMatch(context, input); }
		if (secondChar is '*') { return MultiLineCommentMatch(context, input); }
		return false;
	}

	public static bool CommentStateCheck(Context<Token> context)
	{
		ReadOnlySpan<char> input = context.Input.Span;
		return input[0] is '/' && CommentPatternMatch(context);
	}

	public static bool SingleLineCommentMatch(Context<Token> context, ReadOnlySpan<char> input)
	{
		int index = input.IndexOf('\n');

		int step = index + 1;
		int length = step + 2; // also include the //

		Token token = context.Token;

		token.Type = TokenType.SingleLineComment;
		context.Matched = true;
		context.SkipReturn = true;
		token.Value = index is -1 ?
			context.Advance(amount: context.Input.Length)[2..] :  // the comment is at the end of the file and there is no line feed.
			context.Advance(amount: length, lines: 1, newCurrentLineChar: 0)[2..^1];

		return true;
	}

	public static bool MultiLineCommentMatch(Context<Token> context, ReadOnlySpan<char> input)
	{
		// we need to find '*/'
		if (input.Length < 2) { return false; }

		int length = 2; // also include the '/*'
		int lines = 0;
		int currentLineChar = 0;

		while (true)
		{
			int index = input.IndexOfAny(MultilineCommentStoppers);
			if (index is -1) { return false; }

			char captured = input[index];
			int step = index + 1;

			switch (captured)
			{
				case '\n': lines++; currentLineChar = 0; length += step; break;
				case '\\':
					if (input.Length < step + 1) { return false; }

					if (input[step] is '\n') { lines++; currentLineChar = 0; }
					else { step++; currentLineChar += step; }

					length += step;
					break;
				case '/':
					currentLineChar += step;
					length += step;
					if (index > 0 && input[index - 1] is '*') { goto OUT; }
					break;
				default: throw new UnreachableException();
			}

			input = input[step..];
		}

OUT:
		if (lines is 0) { currentLineChar += context.Char; }

		Token token = context.Token;

		token.Type = TokenType.MultiLineComment;
		token.Value = context.Advance(amount: length, lines, currentLineChar)[2..^2];
		context.Matched = true;
		context.SkipReturn = true;

		return true;
	}

	public static bool DefinitionMatch(Context<Token> context)
	{
		ReadOnlySpan<char> input = context.Input.Span;
		char firstChar = input[0];

		return firstChar is ':' ?
			DefinitionStartMatch(context) :
			DefinitionEndMatch(context);
	}

	public static bool DefinitionStateCheck(Context<Token> context)
	{
		ReadOnlySpan<char> input = context.Input.Span;
		char firstChar = input[0];

		if (firstChar is ':') { return DefinitionStartMatch(context); }
		if (firstChar is ';') { return DefinitionEndMatch(context); }
		return false;
	}

	public static bool DefinitionStartMatch(Context<Token> context)
	{
		ReadOnlySpan<char> input = context.Input.Span;

		if (input.Length < 3) { return false; }

		input = input[..3];
		if (input is not "::=") { return false; }

		Token token = context.Token;
		token.Type = TokenType.DefinitionStart;
		token.Value = context.Advance(amount: 3);
		context.Matched = true;

		return true;
	}

	public static bool DefinitionEndMatch(Context<Token> context)
	{
		Token token = context.Token;
		token.Type = TokenType.DefinitionEnd;
		token.Value = context.Advance(amount: 1);
		context.Matched = true;

		return true;
	}

	public static bool ElementMatch(Context<Token> context)
	{
		ReadOnlySpan<char> input = context.Input.Span;

		// skip the activation symbol
		char firstChar = input[0];
		input = input[1..];

		return firstChar switch
		{
			'<' => NonTerminalElementMatch(context, input),
			'"' => TerminalElementMatch(context, input),
			'(' => GroupingElementStartMatch(context, input),
			')' => GroupingElementEndMatch(context),
			'[' => OptionalElementStartMatch(context),
			']' => OptionalElementEndMatch(context),
			'{' => RepetitionElementStartMatch(context),
			'}' => RepetitionElementEndMatch(context),
			_ => false
		};
	}

	public static bool NonTerminalElementMatch(Context<Token> context, ReadOnlySpan<char> input)
	{
		// we need to find '>'
		if (input.Length < 1) { return false; }

		// non-terminal element names cannot start with a digit
		if (input[0] is >= '0' and <= '9') { return false; }

		int length = 1; // also include the '<'
		int lines = 0;
		int currentLineChar = 0;
		bool wellFormed = true;

		while (true)
		{
			int index = input.IndexOfAny(NonTerminalStoppers);
			if (index is -1) { return false; }

			char captured = input[index];
			int step = index + 1;

			switch (captured)
			{
				case '\n':
					context.Errors.Add(context.File, context.Line + lines, "Non terminal element cannot contains new lines.");
					lines++;
					currentLineChar = 0;
					wellFormed = false;
					break;
				default:
					currentLineChar += step;
					if (captured is '>') { length += step; goto OUT; }
					else { context.Errors.Add(context.File, context.Line + lines, $"Non terminal element cannot contains '{captured}'."); }
					break;
			}

			length += step;
			input = input[step..];
		}

OUT:
		if (lines is 0) { currentLineChar += context.Char; }

		Token token = context.Token;

		token.Type = TokenType.NonTerminalElement;
		token.Value = context.Advance(amount: length, lines, currentLineChar)[1..^1];
		token.WellFormed = wellFormed;
		context.Matched = true;
		context.SkipReturn = true;

		return true;
	}

	public static bool TerminalElementMatch(Context<Token> context, ReadOnlySpan<char> input)
	{
		int length = 1; // include the initial '"' symbol
		while (true)
		{
			// searching for ['"', '\n', '\\']
			int index = input.IndexOfAny(TerminalStoppers);
			char captured = input[index];

			if (index is -1 || captured is '\n') { return false; }

			int step = index + 1;
			if (captured is '"') { length += step; break; }

			// include the special character like '\n' or '\t'
			step++;
			if (input.Length < step) { return false; }

			length += step;
			input = input[step..];
		}

		Token token = context.Token;

		token.Type = TokenType.TerminalElement;
		token.Value = context.Advance(amount: length)[1..^1]; // exclude the '"' symbols
		context.Matched = true;

		return true;
	}

	public static bool GroupingElementStartMatch(Context<Token> context, ReadOnlySpan<char> input)
	{
		// check for explanation comment
		if (input.Length > 0 && input[0] is '*') { return false; }

		Token token = context.Token;

		token.Type = TokenType.GroupingElementStart;
		token.Value = context.Advance(amount: 1);
		context.Matched = true;

		return true;
	}

	public static bool GroupingElementEndMatch(Context<Token> context)
	{
		Token token = context.Token;

		token.Type = TokenType.GroupingElementEnd;
		token.Value = context.Advance(amount: 1);
		context.Matched = true;

		return true;
	}

	public static bool OptionalElementStartMatch(Context<Token> context)
	{
		Token token = context.Token;

		token.Type = TokenType.OptionalElementStart;
		token.Value = context.Advance(amount: 1);
		context.Matched = true;

		return true;
	}

	public static bool OptionalElementEndMatch(Context<Token> context)
	{
		Token token = context.Token;

		token.Type = TokenType.OptionalElementEnd;
		token.Value = context.Advance(amount: 1);
		context.Matched = true;

		return true;
	}

	public static bool RepetitionElementStartMatch(Context<Token> context)
	{
		Token token = context.Token;

		token.Type = TokenType.RepetitionElementStart;
		token.Value = context.Advance(amount: 1);
		context.Matched = true;

		return true;
	}

	public static bool RepetitionElementEndMatch(Context<Token> context)
	{
		Token token = context.Token;

		token.Type = TokenType.RepetitionElementEnd;
		token.Value = context.Advance(amount: 1);
		context.Matched = true;

		return true;
	}

	public static bool WhiteSpaceMatch(Context<Token> context)
	{
		ReadOnlySpan<char> input = context.Input.Span;

		// input[0] is already one of the activation symbols
		int length = 1;
		int lines = input[0] is '\n' ? 1 : 0;
		int currentLineChar = 1;

		while (length < input.Length && WhiteSpaceStoppers.Contains(input[length]))
		{
			if (input[length] is '\n') { lines++; currentLineChar = 0; }
			else { currentLineChar++; }

			length++;
		}

		if (lines is 0) { currentLineChar += context.Char; }

		Token token = context.Token;

		token.Type = TokenType.WhiteSpace;
		token.Value = context.Advance(amount: length, lines, currentLineChar);
		context.Matched = true;
		context.SkipReturn = true;

		return true;
	}

	public static bool WhiteSpaceStateCheck(Context<Token> context)
	{
		ReadOnlySpan<char> input = context.Input.Span;
		return WhiteSpaceStoppers.Contains(input[0]) && WhiteSpaceMatch(context);
	}

	#endregion
}

