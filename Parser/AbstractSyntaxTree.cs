
namespace Parser;

public class AbstractSyntaxTree<T> where T : IComposition
{
	public IAbstractSyntaxNode<T> Root { get; init; }

	public AbstractSyntaxTree(IAbstractSyntaxNode<T> root)
	{
		Root = root;
	}
}

