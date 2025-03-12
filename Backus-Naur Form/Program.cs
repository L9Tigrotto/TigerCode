
using Backus_Naur_Form;
using Backus_Naur_Form.Patterns;
using Lexer;

// Define the file to be processed by the lexer
FileInfo fileInfo = new("BNF.lexer");

string input = File.ReadAllText(fileInfo.FullName);
IPattern<EBNFToken>[] rules = LexerManager.CreatePatterns(out WhiteSpacePattern whiteSpace);

Lexer<EBNFToken> lexer = new(input.AsMemory(), rules, token: new EBNFToken());

try
{
    foreach (EBNFToken token in lexer.Tokenize())
    {
        Console.WriteLine(token);
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error at line {whiteSpace.LineCount}.");
    Console.WriteLine(e);
    throw;
}


/*
 * TODO
 * - create a function that given a .lexer file (written in EBNF),
 *   it creates the patterns
 */