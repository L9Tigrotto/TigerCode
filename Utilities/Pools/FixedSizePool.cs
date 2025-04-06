
namespace Utilities.Pools;

public abstract class FixedSizePool<T>(int size) : IPool<T>
{
	protected Mutex Mutex { get; init; } = new();
	protected T[] Collection { get; init; } = new T[size];
	protected int Count { get; set; } = 0;

	protected abstract T OnEmptyAcquire();
	protected abstract void OnFullRelease(T item);
	protected abstract void OnDispose(T item);

	public T Acquire()
	{
		Mutex.WaitOne();
		if (Count is 0) { return OnEmptyAcquire(); }

		Count--;
		T item = Collection[Count];
		Collection[Count] = default!;

		Mutex.ReleaseMutex();
		return item;
	}

	public void Release(T item)
	{
		Mutex.WaitOne();
		if (Count == Collection.Length) { OnFullRelease(item); return; }

		Collection[Count++] = item;
		Mutex.ReleaseMutex();
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		foreach (T item in Collection) { OnDispose(item); }
	}
}
