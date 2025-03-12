
using Backus_Naur_Form.Patterns;
using Lexer;

namespace Backus_Naur_Form;

/// <summary>
/// A static class that manages the creation of lexer rules for processing Backus-Naur Form (BNF) notation.
/// </summary>
public static class LexerManager
{
    public static IPattern<EBNFToken>[] CreatePatterns(out WhiteSpace whiteSpace)
    {
        whiteSpace = new();

        return [
            whiteSpace,
            new SingleLineComment(),
            new NonTerminal(),
            new StartDefinitionSymbol(),
            new Terminal(),
            new EndDefinitionSymbol(),
            new AlternativeSymbol(),
            new MultiLineComment(),
        ];
    }
}