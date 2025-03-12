
using Lexer;

namespace Backus_Naur_Form.Patterns;

public class AlternativePattern : IPattern<EBNFToken>
{
	private static readonly char[] _activationSymbols = ['|'];

	public char[] ActivationSymbols => _activationSymbols;

	public void ConfirmMatch(MatchDetails<EBNFToken> details)
	{
		details.Token.Type = EBNFTokenType.Alternative;
		details.Input = details.Input[1..];
		details.IsMatch = true;
	}
}
