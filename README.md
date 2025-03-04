# TigerCode: Custom Language Project

Welcome to TigerCode! This repository contains the source code and documentation for my ongoing effort to create a new programming language and provide tools for users to build their own grammars and languages. The project is in its early stages, with the ultimate goal of creating an executable file.

## Project Overview

The aim of TigerCode is twofold:
1. **Develop TigerCode Language**: Create a fully functional programming language from scratch.
2. **Empower Users**: Provide tools for users to define their own grammars and build custom languages.

## Overview of Compiler Architecture

A typical compiler consists of several key stages:
1. Lexical Analysis (Lexical Parsing)
   ##### Primary Responsibilities
   - Breaking input text into meaningful tokens
   - Removing whitespace and comments
   - Identifying individual language elements
   - Preprocessing source code

   ##### Key Tasks
   - Character-level parsing
   - Token classification
   - Literal and identifier recognition
   - Handling escape sequences
   - Detecting and reporting lexical errors

   ##### Example Token Types
   ```
   - KEYWORD       (if, while, return)
   - IDENTIFIER    (variable names, function names)
   - LITERAL       (42, "hello", 3.14)
   - OPERATOR      (+, -, *, /)
   - PUNCTUATION   ({, }, (, ))
   ```
2. Syntax Analysis (Syntactic Parsing)
    ##### Primary Responsibilities
    - Verifying grammatical structure of token sequence
    - Constructing parse trees or abstract syntax trees (AST)
    - Checking if tokens follow language grammar rules
    - Identifying structural errors in code
    
    ##### Parsing Techniques
    1. Recursive Descent Parsing
    2. LL(k) Parsing
    3. LR Parsing
    4. LALR Parsing
    5. Shift-Reduce Parsing
    
    ##### Key Tasks
    - Verify token sequence is grammatically correct
    - Build hierarchical representation of code structure
    - Detect syntax violations
    - Transform tokens into a tree-like structure
    
    #### Example Parse Tree Concept
    ```
    Expression: 3 + 4 * 2
           [Expression]
           /   |   \
        [Term] [+] [Term]
         |          /   \
       [3]      [Term] [*] [Term]
                 |          |
                [4]        [2]
    ```

3. Semantic Analysis (Type Checking)

    ##### Primary Responsibilities
    - Ensuring type consistency
    - Validating semantic rules
    - Symbol table management
    - Type inference and type checking
    
    ##### Key Tasks
    - Verify type compatibility
    - Check variable declarations
    - Ensure proper function calls
    - Resolve symbol references
    - Detect semantic errors
    
    ##### Example Semantic Checks
    ```cs
    // Semantic error detection
    void CheckSemanticRules(AbstractSyntaxTree ast)
    {
        // Check type compatibility
        if (variable.Type != expectedType) { throw new SemanticError("Type mismatch"); }
    
        // Verify function call signatures
        if (functionCall.ArgCount != functionDefinition.ParamCount) { throw new SemanticError("Incorrect argument count"); }
    
        // Validate scope and variable usage
        if (!currentScope.Contains(variable)) { throw new SemanticError("Undefined variable"); }
    }
    ```

4. Intermediate Code Generation (Platform-Independent Code Generation)

    ##### Primary Responsibilities
    - Translating AST into a lower-level, machine-independent representation
    - Preparing code for optimization
    - Creating a universal intermediate format
    
    ##### Common Intermediate Representations
    1. Three-Address Code
    2. Static Single Assignment (SSA)
    3. Quadruples
    4. Abstract Syntax Tree (AST)
    5. Control Flow Graph (CFG)
    
    ##### Example Three-Address Code
    ```
    # Source: x = a + b * c
    t1 = b * c
    t2 = a + t1
    x = t2
    ```

5. Optimization (Performance Enhancement)

    ##### Primary Responsibilities
    - Improving code performance
    - Reducing computational complexity
    - Eliminating redundant computations
    
    ##### Optimization Techniques
    1. Constant Folding
    2. Dead Code Elimination
    3. Loop Optimization
    4. Inline Expansion
    5. Common Subexpression Elimination
    
    ##### Example Optimization
    ```cs
    # Before optimization
    int x = 5 + 3;  # Runtime computation
    
    # After constant folding
    int x = 8;      # Compile-time computation
    ```

6. Code Generation (Platform-Specific Code Generation)

    ##### Primary Responsibilities
    - Translating intermediate representation to target machine code
    - Generating executable instructions
    - Handling target-specific optimizations
    - Memory layout and register allocation
    
    ##### Target Options
    1. Native Machine Code
    2. Bytecode (Java, Python)
    3. Assembly Language
    4. WebAssembly
    5. Transpilation to Another Language
    
    ##### Example Code Generation Strategy
    ```cs
    void GenerateMachineCode(IntermediateRepresentation intermediateRepr)
    {
        // Select appropriate instruction set
        if (targetPlatform == "x86") { GenerateX86Instructions(intermediateRepr); }
        else if (targetPlatform == "ARM") { GenerateArmInstructions(intermediateRepr); }
    }
    ```

## Archived Goals

1. **✅ Initial Setup**: Set up the project repository and initialize basic compiler structure.  
2. **✅ Basic Tokenizer**: Implemented a basic tokenizer to handle simple keywords and identifiers.  
3. **✅ First Grammar File**: Created an initial version of the grammar file for basic syntax.
4. **✅ GitHub Master**: Figure it out to how to close an issue from a commit.

## Current Goals

1. **🟠 Lexer Development**: Build a lexer to tokenize source code.
2. **🔜 Parser Development**: Build a parser to generate an Abstract Syntax Tree (AST).  
3. **🔜 Grammar Extraction**: Read a BNF grammar file to define the TigerCode (or user language) syntax.  
4. **🔜 Grammar Definition**: Formalize valid TigerCode (or user language) grammar rules.  
5. **🔜 Code Translation**: Convert parsed TigerCode into C source code, which can be compiled using an external C compiler.  

## Future Goals
- **🟡 Parse Tree Visualizer**: Create a visual tool to help with debugging the parse tree, offering real-time insight into the structure of the source code.
- **🟡 Error Handling**: Improve error reporting across lexical, syntactic, and semantic analysis to make the compiler more user-friendly.
- **🟡 NuGet Package**: Publish the Lexer and Parser (and other compiler components) as a NuGet package for simple installation and use in projects.
- **🟡 Standard Library for TigerCode**: Implement a set of basic functions (I/O, math, memory management) for the TigerCode language to enable easier development.
- **🟡 Self-contained Compilation**: Implement parts 3, 4, 5, and 6 of the compiler (Intermediate Code Generation, Optimization, Code Generation, and Platform-Specific Code Generation) to **eliminate the need to rely on an external C compiler**. This will enable TigerCodeCompiler to become a fully self-contained compiler capable of compiling TigerCode directly to machine code or intermediate representations like bytecode.

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
