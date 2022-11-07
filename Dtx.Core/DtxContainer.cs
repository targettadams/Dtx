using Microsoft.Extensions.Logging;

namespace Dtx.Core;

public class DtxContainer<T> : IDisposable where T : class, IDtxRow<T>, new()
{
    private readonly ILogger _logger;

    private IDtxStreamReader? _dtxStreamReader = null;

    private IEnumerable<T>? _contents; 

    public IEnumerable<T>? Contents
    {
        get => _contents;
        set => _contents = value;
    }

    public DtxContainer(ILogger logger, string filePath, char delimiter = ',')
    {
        _logger = logger;
        Configure(filePath, delimiter);
    }

    public DtxContainer(string filePath, char delimiter = ',') 
    {
        _logger = new LoggerFactory().CreateLogger(string.Empty); // No logging.
        Configure(filePath, delimiter);
    }

    private void Configure(string filePath, char delimiter = ',')
    {
        IDtxConfiguration configuration = new DtxConfiguration(filePath) { Delimiter = delimiter };
        _dtxStreamReader = new DtxStreamReader(_logger, configuration);
        Contents = new DtxEnumerable<T>(_dtxStreamReader, configuration);
    }

    public void Dispose()
    {
        if (_dtxStreamReader is not null)
        {
            _dtxStreamReader.Dispose();
        }
    }
}
