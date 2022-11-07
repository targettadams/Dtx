namespace Dtx.Core;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DtxHeadingAttribute : Attribute
{
    public string? Value { get; set; }

    public DtxHeadingAttribute()
    {
        Value = null;
    }

    public DtxHeadingAttribute(string value)
    {
        Value = value;
    }   
}
