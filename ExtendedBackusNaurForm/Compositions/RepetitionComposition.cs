
using Parser;

namespace ExtendedBackusNaurForm.Compositions;

public class RepetitionComposition : IComposition
{
	public LinkedList<IComposition> Compositions { get; init; }

	public RepetitionComposition()
	{
		Compositions = [];
	}
}

