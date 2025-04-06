

namespace Utilities.Pools;

public abstract class SingleItemPool<T> : IPool<T>
{
	protected T? Item { get; set; } = default;
	public bool IsItemSet { get; set; } = false;

	protected abstract T OnEmptyAcquire();
	protected abstract void OnFullRelease(T item);
	protected abstract void OnDispose(T item);

	public T Acquire()
	{
		if (!IsItemSet) { return OnEmptyAcquire(); }

		T item = Item!;
		Item = default;
		IsItemSet = false;

		return item;
	}

	public void Release(T item)
	{
		if (IsItemSet) { OnFullRelease(item); return; }

		Item = item;
		IsItemSet = true;
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		if (IsItemSet) { OnDispose(Item!); }
	}
}

