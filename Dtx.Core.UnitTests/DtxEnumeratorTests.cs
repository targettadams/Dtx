using Dtx.TestUtils;
using Moq;

namespace Dtx.Core.UnitTests;

public class DtxEnumeratorTests
{
    [Fact]
    public void MoveNext_WithRedundantColumn_FirstRowReturned()
    {
        // ------- Arrange.
        Mock<IDtxStreamReader> reader = new();
        reader.SetupSequence(i => i.ReadLine())
            .Returns("First Name,Middle name,LastName,Age")
            .Returns("John,,Major,79")
            .Returns("Tony,,Blair,69")
            .Returns("Liz,,Truss,47");

        DtxEnumerable<PrimeMinister> enumerable = new(reader.Object, new DtxConfiguration(string.Empty));

        IEnumerator<PrimeMinister> enumerator = enumerable.GetEnumerator();

        // ------- Act.
        enumerator.MoveNext();

        // ------- Assert.
        string? firstName = enumerator.Current.FirstName;
        string? lastName = enumerator.Current.LastName;
        int age = enumerator.Current.Age;

        Assert.True(firstName == "John");
        Assert.True(lastName == "Major");
        Assert.True(age == 79);
    }

    [Fact]
    public void MoveNext_ThreeTimesWithPercentDelimiter_FirstRowReturned()
    {
        // ------- Arrange.
        Mock<IDtxStreamReader> reader = new();
        reader.SetupSequence(i => i.ReadLine())
            .Returns("First Name%LastName%Age")
            .Returns("John%Major%79")
            .Returns("Tony%Blair%69")
            .Returns("Liz%Truss%47");

        DtxEnumerable<PrimeMinister> enumerable = new(reader.Object, new DtxConfiguration(string.Empty) { Delimiter = '%' });

        IEnumerator<PrimeMinister> enumerator = enumerable.GetEnumerator();

        // ------- Act.
        enumerator.MoveNext();
        enumerator.MoveNext();
        enumerator.MoveNext();

        // ------- Assert.
        string? firstName = enumerator.Current.FirstName;
        string? lastName = enumerator.Current.LastName;
        int age = enumerator.Current.Age;

        Assert.True(firstName == "Liz");
        Assert.True(lastName == "Truss");
        Assert.True(age == 47);
    }

    [Fact]
    public void MoveNext_TwiceWithTabDelimiter_FirstRowReturned()
    {
        // ------- Arrange.
        Mock<IDtxStreamReader> reader = new();
        reader.SetupSequence(i => i.ReadLine())
            .Returns("First Name\tLastName\tAge")
            .Returns("John\tMajor\t79")
            .Returns("Tony\tBlair\t69")
            .Returns("Liz\tTruss\t47");

        DtxEnumerable<PrimeMinister> enumerable = new(reader.Object, new DtxConfiguration(string.Empty) { Delimiter = '\t' });

        IEnumerator<PrimeMinister> enumerator = enumerable.GetEnumerator();

        // ------- Act.
        enumerator.MoveNext();
        enumerator.MoveNext();

        // ------- Assert.
        string? firstName = enumerator.Current.FirstName;
        string? lastName = enumerator.Current.LastName;
        int age = enumerator.Current.Age;

        Assert.True(firstName == "Tony");
        Assert.True(lastName == "Blair");
        Assert.True(age == 69);
    }

    [Fact]
    public void MoveNext_Once_FirstRowReturned()
    {
        // ------- Arrange.
        Mock<IDtxStreamReader> reader = new();
        reader.SetupSequence(i => i.ReadLine())
            .Returns("First Name,LastName,Age")
            .Returns("John,Major,79")
            .Returns("Tony,Blair,69")
            .Returns("Liz,Truss,47");

        DtxEnumerable<PrimeMinister> enumerable = new(reader.Object, new DtxConfiguration(string.Empty));

        IEnumerator<PrimeMinister> enumerator = enumerable.GetEnumerator();

        // ------- Act.
        enumerator.MoveNext();

        // ------- Assert.
        string? firstName = enumerator.Current.FirstName;
        string? lastName = enumerator.Current.LastName;
        int age = enumerator.Current.Age;

        Assert.True(firstName == "John");
        Assert.True(lastName == "Major");
        Assert.True(age == 79);
    }

    [Fact]
    public void MoveNext_ThreeTimes_ThirdRowReturned()
    {
        // ------- Arrange.
        Mock<IDtxStreamReader> reader = new();
        reader.SetupSequence(i => i.ReadLine())
            .Returns("First Name,LastName,Age")
            .Returns("John,Major,79")
            .Returns("Tony,Blair,69")
            .Returns("Liz,Truss,47");

        DtxEnumerable<PrimeMinister> enumerable = new(reader.Object, new DtxConfiguration(string.Empty));

        IEnumerator<PrimeMinister> enumerator = enumerable.GetEnumerator();

        // ------- Act.
        for (int i=0;i<3;i++) enumerator.MoveNext();

        // ------- Assert.
        string? firstName = enumerator.Current.FirstName;
        string? lastName = enumerator.Current.LastName;
        int age = enumerator.Current.Age;

        Assert.True(firstName == "Liz");
        Assert.True(lastName == "Truss");
        Assert.True(age == 47);
    }

}
