
namespace Utilities.Pools;

public abstract class LinkedPool<T> : IPool<T>
{
	protected Mutex Mutex { get; init; } = new();
	protected LinkedList<T> Collection { get; init; } = [];

	protected abstract T OnEmptyAcquire();
	protected abstract void OnDispose(T item);

	public T Acquire()
	{
		Mutex.WaitOne();
		if (Collection.Count is 0) { return OnEmptyAcquire(); }

		T item = Collection.First!.Value;
		Collection.RemoveFirst();

		Mutex.ReleaseMutex();
		return item;
	}

	public void Release(T item)
	{
		Mutex.WaitOne();
		Collection.AddLast(item);
		Mutex.ReleaseMutex();
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);

		LinkedListNode<T>? current = Collection.First!;
		
		while (current is not null)
		{
			OnDispose(current.Value);
			current = current.Next;
		}

		Collection.Clear();
	}
}

