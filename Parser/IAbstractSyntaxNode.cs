
namespace Parser;

public interface IAbstractSyntaxNode<T> where T : IComposition
{
	public IAbstractSyntaxNode<T>? Parent { get; init; }
	public LinkedList<IComposition> Children { get; init; }
}

