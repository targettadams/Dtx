using Microsoft.Extensions.Logging;

namespace Dtx;

public class TextFileLogger : ILogger
{
    private readonly StreamWriter _writer;

    public TextFileLogger(StreamWriter writer)
    {
        _writer = writer;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return _writer;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Information:
            case LogLevel.Trace:
            case LogLevel.None:
            case LogLevel.Debug:
            case LogLevel.Warning:
            case LogLevel.Error:
            case LogLevel.Critical:
            default:
                return true;
        }
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel))
        {
            _writer.Write($"Level: {logLevel}; Event Id: {eventId.Id}");

            if (state != null)
            {
                _writer.Write($"; State: {state}");
            }

            if (exception != null)
            {
                _writer.Write($"; Exception: {exception.Message}");
            }

            _writer.WriteLine();
        }
    }
}
