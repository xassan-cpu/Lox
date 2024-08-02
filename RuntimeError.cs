namespace Lox;

public class RunTimeError : Exception
{
    public readonly Token token;
    public readonly string message;

    public RunTimeError(Token token, string message)
    {
        this.token = token;
        this.message = message;
    }

    public string GetMessage()
    {
        return message;
    }
}