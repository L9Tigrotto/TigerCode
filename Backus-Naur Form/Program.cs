
using Backus_Naur_Form;
using Backus_Naur_Form.Patterns;
using Lexer;

// Define the file to be processed by the lexer
FileInfo fileInfo = new("BNF.lexer");

string input = File.ReadAllText(fileInfo.FullName);
IPattern<EBNFToken>[] rules = LexerManager.CreatePatterns(out WhiteSpace whiteSpace);

Lexer<EBNFToken> lexer = new(input.AsMemory(), rules, token: new EBNFToken());

foreach (EBNFToken token in lexer.Tokenize())
{
    Console.WriteLine(token);
}

/*
 * TODO
 * - create missing token type (optional, repetition, grouping)
 * - create a function that given a .lexer file (written in EBNF),
 *   it creates the patterns
 */