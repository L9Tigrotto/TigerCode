
using System.Diagnostics;

namespace Lexer;

public class Context<T>
{
	public FileInfo File { get; set; }
	public ReadOnlyMemory<char> Input { get; set; }

	public int Line { get; set; }
	public int Char { get; set; }

	public ErrorCollection Errors { get; init; }

	public bool Matched { get; set; }
	public bool SkipReturn { get; set; }
	public T Token { get; set; }

	public Context(FileInfo file, ErrorCollection errors, T token)
	{
		File = file;
		string input = System.IO.File.ReadAllText(file.FullName);
		Input = input.AsMemory();

		Line = 1;
		Char = 1;

		Errors = errors;

		Matched = false;
		SkipReturn = false;
		Token = token;
	}

	public ReadOnlyMemory<char> Advance(int amount)
	{
		Debug.Assert(amount <= Input.Length);

		ReadOnlyMemory<char> matched = Input[..amount];
		Input = Input[amount..];

		Char += amount;

		return matched;
	}

	public ReadOnlyMemory<char> Advance(int amount, int lines, int newCurrentLineChar)
	{
		Debug.Assert(amount <= Input.Length);

		ReadOnlyMemory<char> matched = Input[..amount];
		Input = Input[amount..];

		Line += lines;
		Char = newCurrentLineChar;

		return matched;
	}

	public void ResetFlags()
	{
		Matched = false;
		SkipReturn = false;
	}

	public void ChangeFile(FileInfo file)
	{
		File = file;
		string input = System.IO.File.ReadAllText(file.FullName);
		Input = input.AsMemory();

		Line = 1;
		Char = 1;
	}
}