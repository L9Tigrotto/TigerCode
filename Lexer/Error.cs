namespace Lexer;

public class Error(FileInfo file, int line, string message)
{
	public FileInfo File { get; set; } = file;
	public int Line { get; init; } = line;
	public string Message { get; set; } = message;
}