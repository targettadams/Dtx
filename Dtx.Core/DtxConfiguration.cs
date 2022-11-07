namespace Dtx.Core;

public class DtxConfiguration : IDtxConfiguration
{
    public string FilePath { get; init; } 

    public char Delimiter { get; init; } = ',';

    public DtxConfiguration(string filePath)
    {
        FilePath = filePath;
    }

}
