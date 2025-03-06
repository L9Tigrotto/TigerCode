# TigerCode: Custom Language Project

Welcome to TigerCode! This repository contains the source code and documentation for my ongoing effort to create a new programming language and provide tools for users to build their own grammars and languages. The project is in its early stages.

## Project Overview

The aim of TigerCode is twofold:
1. **Develop TigerCode Language**: Create a fully functional programming language from scratch.
2. **Empower Users**: Provide tools for users to define their own grammars and build custom languages.

## Status Legend
| Symbol | Status | Description |
|--------|--------|-------------|
| ✅ | Completed | Tasks that have been successfully implemented and tested |
| 🟠 | In Progress | Currently being worked on and actively developed |
| 🔜 | Planned (Short-term) | Next in the development queue, requires completion of one or more in-progress (🟠) items |
| 🟡 | Planned (Future) | Features and improvements planned for future development cycles |

## Archived Goals

1. **✅ Initial Setup**: Set up the project repository and initialize basic compiler structure.  
2. **✅ Basic Tokenizer**: Implemented a basic tokenizer to handle simple keywords and identifiers.  
3. **✅ First Grammar File**: Created an initial version of the grammar file for basic syntax.
4. **✅ GitHub Master**: Figure it out to how to close an issue from a commit.

## Current Goals

1. **🟠 Lexer Development**: Build a lexer to tokenize source code.
2. **🔜 Parser Development**: Build a parser to generate an Abstract Syntax Tree (AST).  
3. **🔜 Grammar Extraction**: Read a BNF grammar file to define the TigerCode (or user language) syntax.  
4. **🔜 Grammar Definition**: Formalize valid TigerCode grammar rules.  
5. **🔜 Code Translation**: Convert parsed TigerCode into C source code, which can be compiled using an external C compiler.  

## Future Goals
- **🟡 Parse Tree Visualizer**: Create a visual tool to help with debugging the parse tree, offering real-time insight into the structure of the source code.
- **🟡 Error Handling**: Improve error reporting across lexical, syntactic, and semantic analysis to make the compiler more user-friendly.
- **🟡 NuGet Package**: Publish the Lexer and Parser (and other compiler components) as a NuGet package for simple installation and use in projects.
- **🟡 Standard Library for TigerCode**: Implement a set of basic functions (I/O, math, memory management) for the TigerCode language to enable easier development.
- **🟡 Self-contained Compilation**: Implement parts 3, 4, 5, and 6 of the [CompilerOverview](https://github.com/L9Tigrotto/TigerCode/blob/doc/redme_and_compiler_overview/CompilerOverview.md) (Intermediate Code Generation, Optimization, Code Generation, and Platform-Specific Code Generation) to **eliminate the need to rely on an external C compiler**. This will enable TigerCodeCompiler to become a fully self-contained compiler capable of compiling TigerCode directly to machine code or intermediate representations like bytecode.

## Getting Started

### [WIP] Create Your Own Grammar and Compile Projects

1. **Clone the Repository**:
   - Clone the repository using the following command: `git clone https://github.com/yourusername/TigerCode.git`
   - Navigate to the project directory: `cd TigerCode`
   - Alternatively, download it from NuGet packages.

2. **Write Your Own Grammar**:
   - Define your grammar rules and save them in a file.
   - Pass this grammar file to the lexer.

3. **Write a Program**:
   - Create a program in your new language.
   - Provide this program to the parser for compilation.

### [WIP] Compile TigerCode Files

1. **Download the TigerCode Compiler**:
   - Obtain the TigerCode compiler from the repository or future distribution channels.

2. **Execute the Compiler**:
   - Run the compiler, passing your TigerCode file(s) as arguments.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Contact

If you have any questions or suggestions, feel free to open an issue.

Happy coding!
