using Dtx.Core;

namespace Dtx.TestUtils;

public class PrimeMinister : IDtxRow<PrimeMinister>
{
    [DtxHeading("First Name")]
    public string? FirstName { get; set; }

    [DtxHeading("LastName")]
    public string? LastName { get; set; }

    [DtxHeading]
    public int Age { get; set; }

    public IDtxRow<PrimeMinister> Build(IDictionary<string, string> keyValuePairs)
    {
        return new PrimeMinister()
        {
            FirstName = keyValuePairs[nameof(FirstName)],
            LastName = keyValuePairs[nameof(LastName)],
            Age = int.Parse(keyValuePairs[nameof(Age)])
        };
    }
}