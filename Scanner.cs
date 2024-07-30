using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Lox;

class Scanner
{
    private readonly string source;
    private readonly List<Token> tokens = new();

    private int start = 0;
    private int current = 0;
    private int line = 1;

    public Scanner(string source)
    {
        this.source = source;
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd()) 
        {
            // We are at the beginning of the lexeme.
            start = current;
            ScanToken();
        }

        tokens.Add(new Token(TokenType.EOF, "", null, line)); 
        return tokens;
    }

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            case '!':
                AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '/':
                if (Match('/'))
                {
                    // A comment goes until the end of the line.
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;

            case ' ':
            case '\t':
            case '\r':
                // Ignore whitespace.
                break;

            case '\n':
                line++;
                break;

            default:
                Lox.Error(line, "Unexpected character.");
                break;
        }
    }

    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false;

        current++;
        return true;
    }

    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return source[current];
    }

    private bool IsAtEnd()
    {
        return current >= source.Length;
    }

    private char Advance()
    {
        current++;
        return source[current - 1];
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, object? literal)
    {
        string text = source[start..current];
        tokens.Add(new Token(type, text, literal, line));
    }
}