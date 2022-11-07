using Dtx.Core;

namespace Dtx.TestUtils;

public class PrimeMinisterMissingAttribute : IDtxRow<PrimeMinisterMissingAttribute>
{
    [DtxHeading("First Name")]
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [DtxHeading]
    public int Age { get; set; }

    public IDtxRow<PrimeMinisterMissingAttribute> Build(IDictionary<string, string> keyValuePairs)
    {
        throw new NotImplementedException();
    }
}