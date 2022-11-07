using Dtx.TestUtils;

namespace Dtx.Core.UnitTests;

public class DtxHeadingAttributeTests
{
    /// <summary>
    /// Check that DtxUtils.DtxAttributePropertyMap only returns entries for decorated properties.   
    /// </summary>
    [Fact]
    public void AttributePropertyMapTest_Alpha()
    {
        // ------- Act.
        var attributePropertyMap = DtxUtils.DtxAttributePropertyMap<PrimeMinister>();

        // ------- Assert.
        Assert.True(attributePropertyMap.Count == 3);
    }

    /// <summary>
    /// Check that DtxUtils.DtxAttributePropertyMap only returns entries for decorated properties.   
    /// </summary>
    [Fact]
    public void AttributePropertyMapTest_Beta()
    {
        // ------- Act.
        var attributePropertyMap = DtxUtils.DtxAttributePropertyMap<PrimeMinisterMissingAttribute>();

        // ------- Assert.
        Assert.False(attributePropertyMap.Count == 3);
    }
}