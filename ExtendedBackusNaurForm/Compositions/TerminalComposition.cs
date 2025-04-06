
using Parser;

namespace ExtendedBackusNaurForm.Compositions;

public class TerminalComposition : IComposition
{
	public ReadOnlyMemory<char> Value { get; set; }
}

