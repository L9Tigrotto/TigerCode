namespace Lexer;

public class ErrorCollection
{
	protected Mutex Mutex { get; set; } = new();
	protected LinkedList<Error> Errors { get; init; } = [];

	public void Add(FileInfo file, int line, string message)
	{
		Error error = new(file, line, message);

		Mutex.WaitOne();
		Errors.AddLast(error);
		Mutex.ReleaseMutex();
	}

	public void Foreach(Action<Error> action)
	{
		Mutex.WaitOne();

		LinkedListNode<Error>? current = Errors.First;

		while (current is not null)
		{
			action(current.Value);
			current = current.Next;
		}

		Mutex.ReleaseMutex();
	}
}

