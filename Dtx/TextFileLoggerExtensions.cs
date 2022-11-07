using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dtx;

public static class TextFileLoggerExtensions
{

    public static ILoggingBuilder AddTextFileLogger(
    this ILoggingBuilder builder, string logFile)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider>(
                new Func<IServiceProvider, TextFileLoggerProvider>(service => new TextFileLoggerProvider(logFile))));

        //LoggerProviderOptions.RegisterProviderOptions
        //    <ColorConsoleLoggerConfiguration, ColorConsoleLoggerProvider>(builder.Services);

        return builder;
    }

}
