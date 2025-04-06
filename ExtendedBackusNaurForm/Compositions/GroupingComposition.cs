
using Parser;

namespace ExtendedBackusNaurForm.Compositions;

public class GroupingComposition : IComposition
{
	public LinkedList<IComposition> Compositions { get; init; }

	public GroupingComposition()
	{
		Compositions = [];
	}
}

