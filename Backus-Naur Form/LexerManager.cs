
using Backus_Naur_Form.Patterns;
using Lexer;

namespace Backus_Naur_Form;

/// <summary>
/// A static class that manages the creation of lexer rules for processing Backus-Naur Form (BNF) notation.
/// </summary>
public static class LexerManager
{
    public static IPattern<EBNFToken>[] CreatePatterns(out WhiteSpacePattern whiteSpacePattern)
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
}