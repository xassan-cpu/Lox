namespace Lox;

public class Lox
{
    private static readonly Interpreter interpreter = new();
    static bool hadError = false;
    static bool hadRuntimeError = false;

    public static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: lox [script]");
            Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
    }

    private static void RunFile(string path)
    {
        StreamReader sr = new(path);
        Run(sr.ReadToEnd());

        // Indicate an error in the exit.
        if (hadError) Environment.Exit(65);
        if (hadRuntimeError) Environment.Exit(70);
    }

    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            Run(Console.ReadLine());
            hadError = false;            
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new(source);
        List<Token> tokens = scanner.ScanTokens();

        Parser parser = new(tokens);
        Expr expression = parser.Parse();

        // Stop if there was a syntax error.
        if (hadError) return;

        interpreter.Interpret(expression);
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
        hadError = true;
    }

    public static void Error(Token token, string message)
    {
        if (token.type == TokenType.EOF)
        {
            Report(token.line, " at end", message);
        }
        else
        {
            Report(token.line, $" at '{token.lexeme}'", message);
        }
    }

public static void RuntimeError(RunTimeError error)
    {
        Console.WriteLine(error.GetMessage() + "\n[line " + error.token.line + "]");
        hadRuntimeError = true;
    }
}