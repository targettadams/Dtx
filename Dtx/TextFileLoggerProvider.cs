using Microsoft.Extensions.Logging;

namespace Dtx;

public class TextFileLoggerProvider : ILoggerProvider
{
    private readonly StreamWriter _writer;

    public TextFileLoggerProvider(string logFile)
    {
        _writer = new StreamWriter(File.Open(logFile, FileMode.Create, FileAccess.Write));
    }   

    public ILogger CreateLogger(string categoryName)
    {
        return new TextFileLogger(_writer);
    }

    public void Dispose() 
    { 
        _writer.Dispose();
    }
}
