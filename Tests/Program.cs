using Lexer;

FileInfo fileInfo = new(fileName: "Test.bnf");
Rule<MyToken> skip = new SkipRule<MyToken>([" ", "\n", "\r\n"]);
List<Rule<MyToken>> rules = [];


Lexer<MyToken> lexer = Lexer<MyToken>.From(fileInfo, skip, rules);

public enum MyToken
{
    NULL,
}