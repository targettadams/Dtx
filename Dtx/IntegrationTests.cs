using Dtx.Core;
using Dtx.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dtx
{
    public static class IntegrationTests
    {
        public static bool RunTests(IServiceProvider container)
        {
            if (!RunCommaDelimitedTest(Path.Combine(Environment.CurrentDirectory, "Data", "CommaSeperatedTestData.csv")))
            {
                return false;
            }

            if (!RunTabDelimitedTest(Path.Combine(Environment.CurrentDirectory, "Data", "TabSeperatedTestData.csv")))
            {
                return false;
            }

            if (!AutoFacDisposeTest(container, Path.Combine(Environment.CurrentDirectory, "Data", "CommaSeperatedTestData.csv")))
            {
                return false;
            }

            return true;
        }

        public static bool AutoFacDisposeTest(IServiceProvider services, string filename)
        {
            using (DtxContainer<PrimeMinister> container = new DtxContainer<PrimeMinister>(
                services.GetRequiredService<ILogger<Program>>(), filename)) 
            {
                if (container.Contents is null)
                {
                    throw new InvalidOperationException("Contents of Dtx container are null.");
                }

                IEnumerator<PrimeMinister> enumerator = container.Contents.GetEnumerator();

                enumerator.MoveNext();
            }

            return true;
        }

        public static bool RunTabDelimitedTest(string filename)
        {
            bool pass = false;

            using (DtxContainer<PrimeMinister> container = new(filename, '\t'))
            {
                IEnumerable<PrimeMinister>? contents = container.Contents;

                if (contents is null)
                {
                    return false;
                }

                if (contents.Count() == 7)
                {
                    pass = true;
                }
            }

            return pass;
        }

        public static bool RunCommaDelimitedTest(string filename)
        {
            bool pass = false;

            using (DtxContainer<PrimeMinister> container = new(filename))
            {
                IEnumerable<PrimeMinister>? contents = container.Contents;

                if (contents is null)
                {
                    return false;
                }

                if (contents.Count() == 7)
                {
                    pass = true;
                }
            }

            return pass;
        }

    }
}
