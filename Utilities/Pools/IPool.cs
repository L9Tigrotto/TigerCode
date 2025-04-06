namespace Utilities.Pools;

public interface IPool<T> : IDisposable
{
	public T Acquire();
	public void Release(T item);
}

