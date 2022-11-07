using System.Reflection;

namespace Dtx.Core;

public interface IDtxRow<T>
{
    IDtxRow<T> Build(IDictionary<string, string> keyValuePairs);
}

public interface IDtxStreamReader : IDisposable
{
    string? ReadLine();

    void Reset();

    void Open();

    void Close();
}

public interface IDtxConfiguration
{
    string FilePath { get; }

    char Delimiter { get; }
}

public static class DtxUtils
{
    public static IDictionary<string, string> DtxAttributePropertyMap<T>() where T : IDtxRow<T>
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        IDictionary<string, string> attributePropertyMap = new Dictionary<string, string>();

        foreach (PropertyInfo property in properties)
        {
            DtxHeadingAttribute? attribute = property.GetCustomAttribute<DtxHeadingAttribute>();
            if (attribute != null)
            {
                attributePropertyMap.Add(attribute.Value ?? property.Name, property.Name);
            }
        }

        return attributePropertyMap;
    }
}
