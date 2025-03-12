
using Backus_Naur_Form.Patterns;
using Lexer;

namespace Backus_Naur_Form;

public static class PatternManager
{
	public static IPattern<EBNFToken>[] CreateEBNFPatterns(out WhiteSpacePattern whiteSpacePattern)
	{
		whiteSpacePattern = new();

		return [
			whiteSpacePattern,
			new ElementPattern(),
			new CommentPattern(),
			new AlternativePattern(),
			new DefinitionPattern(),
		];
	}

	public static IPattern<EBNFToken>[] CreateCustomGrammarPatterns(FileInfo fileInfo)
	{
		if (!fileInfo.Exists) { return []; }

		IPattern<EBNFToken>[] rules = CreateEBNFPatterns(out WhiteSpacePattern whiteSpace);
		Lexer<EBNFToken> lexer = new(fileInfo, rules, token: new EBNFToken());

		foreach (EBNFToken token in lexer.Tokenize())
		{
			
		}

		return [];
	}
}