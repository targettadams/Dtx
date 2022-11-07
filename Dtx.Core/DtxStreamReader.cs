using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Dtx.Core;

public class DtxStreamReader : IDtxStreamReader
{
    public ILogger Logger { get; init; }

    public IDtxConfiguration Configuration { get; init; }

    private bool _closed = true;

    private FileStream _stream = null!;

    private StreamReader _streamReader = null!;

    public DtxStreamReader(ILogger logger, IDtxConfiguration configuration)
    {
        Logger = logger;
        Configuration = configuration;
    }

    public void Reset()
    {
        if (!_closed) Close();
    }

    public void Dispose()
    {
        Logger.LogInformation("Calling DtxStreamReader.Dispose().");

        if (!_closed) Close();

        _streamReader?.Dispose();
        _stream?.Dispose();
    }

    public void Open()
    {
        if (_closed)
        {
            Logger.LogInformation($"Opening stream ... Using file: {Configuration.FilePath}");

            _stream = new(Configuration.FilePath, FileMode.Open, FileAccess.Read);
            _streamReader = new(_stream);
            _closed = false;
        }
    }

    public void Close()
    {
        if (!_closed)
        {
            Logger.LogInformation($"Closing stream: {Configuration.FilePath}");

            _streamReader.Close();
            _stream.Close();
            _closed = true;
        }
    }

    public string? ReadLine()
    {
        if (_closed) Open();

        return _streamReader.ReadLine();
    }
}
