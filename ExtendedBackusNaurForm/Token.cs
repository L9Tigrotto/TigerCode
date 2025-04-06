
namespace ExtendedBackusNaurForm;

public enum TokenType
{
	Empty,
	WhiteSpace,

	SingleLineComment,
	MultiLineComment,

	NonTerminalElement,
	TerminalElement,

	DefinitionStart,
	Alternative,
	DefinitionEnd,

	OptionalElementStart,
	OptionalElementEnd,

	RepetitionElementStart,
	RepetitionElementEnd,

	GroupingElementStart,
	GroupingElementEnd,
}

public class Token(TokenType type, ReadOnlyMemory<char> value, bool wellFormed)
{
	public TokenType Type { get; set; } = type;
	public ReadOnlyMemory<char> Value { get; set; } = value;
	public bool WellFormed { get; set; } = wellFormed;

	public Token(TokenType type) : this(type, value: ReadOnlyMemory<char>.Empty, wellFormed: false) { }
	public Token() : this(type: TokenType.Empty, value: ReadOnlyMemory<char>.Empty, wellFormed: false) { }

	public void Reset()
	{
		Type = TokenType.Empty;
		Value = ReadOnlyMemory<char>.Empty;
	}

	public override string ToString()
	{
		return Value.IsEmpty
			? $"Type: {Type}"
			: $"Type: {Type,-30} Value: {Value}";
	}
}
