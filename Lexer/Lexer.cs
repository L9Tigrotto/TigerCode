
using System.Diagnostics.CodeAnalysis;

namespace Lexer;

public abstract class Lexer<T>
{
	protected Context<T> Context { get; set; }

	protected Lexer(FileInfo file, ErrorCollection errors)
	{
		Context = new(file, errors, token: GenerateEmptyToken());
	}

	//protected FileInfo File => Context.File;
	//protected ReadOnlyMemory<char> Input => Context.Input;
	//protected ErrorCollection Errors => Context.Errors;

	public int LineNumber => Context.Line;
	public int CurrentLineChar => Context.Char;

	protected abstract T GenerateEmptyToken();
	public abstract bool NextToken([NotNullWhen(true)] out T? token);

	public void ChangeFile(FileInfo file) { Context.ChangeFile(file); }
}
