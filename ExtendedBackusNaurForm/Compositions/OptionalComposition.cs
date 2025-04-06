
using Parser;

namespace ExtendedBackusNaurForm.Compositions;

public class OptionalComposition : IComposition
{
	public LinkedList<IComposition> Compositions { get; init; }

	public OptionalComposition()
	{
		Compositions = [];
	}
}

