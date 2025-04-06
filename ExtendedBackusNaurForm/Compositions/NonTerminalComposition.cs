
using Parser;

namespace ExtendedBackusNaurForm.Compositions;

public class NonTerminalComposition : IComposition
{
	public ReadOnlyMemory<char> Name { get; init; } 

	public LinkedList<IComposition> Values { get; init; }

	public NonTerminalComposition(ReadOnlyMemory<char> name)
	{
		Name = name;
		Values = [];
	}
}
