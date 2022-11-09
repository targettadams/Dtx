using System.Collections;
using System.Diagnostics;

namespace Dtx.Core;

public class DtxEnumerable<T> : IEnumerable<T> where T: class, IDtxRow<T>, new()
{
    private readonly IDtxStreamReader _reader;

    private readonly char _delimiter;

    public DtxEnumerable(IDtxStreamReader reader, IDtxConfiguration configuration)
    {
        Debug.Assert(reader != null);

        _reader = reader;
        _delimiter = configuration.Delimiter;
    }

    public IEnumerator<T> GetEnumerator()
    {
        DtxEnumerator<T> enumerator = new(_reader, _delimiter);
        enumerator.Reset();

        return enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (IEnumerator)GetEnumerator();
    }
}

public class DtxEnumerator<T> : IEnumerator<T> where T : class, IDtxRow<T>, new()
{
    const int START_LINE = -2; // -2 because of header line. Lines are then 0 index based.

    private static T DEFAULT_OBJECT = new(); 

    private readonly IDtxStreamReader _reader;

    private readonly char _delimiter;

    private int _line = START_LINE;

    private bool _enumerationFinished = false;

    private string[]? _dtxHeaders;

    private List<int> _dtxHeaderIndices = new List<int>();

    private readonly IDictionary<string, string> _keyValuePairs = new Dictionary<string, string>();

    T? _dtxRow = null;

    public DtxEnumerator(IDtxStreamReader reader, char delimiter)
    {
        Debug.Assert(reader != null);

        _reader = reader;
        _delimiter = delimiter;
    }

    public T Current
    {
        get
        {
            if (_dtxRow == null)
            {
                throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
            }

            if (_enumerationFinished)
            {
                throw new InvalidOperationException("Enumeration already finsihed.");
            }

            return _dtxRow;
        }
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public void Dispose()
    {
        _reader.Close();
    }

    public bool MoveNext()
    {
        if (!_enumerationFinished)
        {
            if (_line == START_LINE)
            {
                string? headerLine = _reader.ReadLine();

                if (headerLine is not null)
                {
                    _line++;

                    _dtxHeaders = headerLine.Split(_delimiter);

                    IDictionary<string, string> attributePropertyMap = DtxUtils.DtxAttributePropertyMap<T>();

                    _dtxHeaderIndices.Clear();

                    for (int i = 0; i < _dtxHeaders.Length; i++)
                    {
                        if (attributePropertyMap.ContainsKey(_dtxHeaders[i]))
                        {
                            _dtxHeaders[i] = attributePropertyMap[_dtxHeaders[i]];
                            _dtxHeaderIndices.Add(i);
                        }
                    }
                }
            }

            if ( (_dtxHeaders is null) || (_dtxHeaderIndices.Count == 0) )
            {
                throw new InvalidOperationException("No valid header line defined.");
            }

            string? nextLine = _reader.ReadLine();
            if (nextLine is not null)
            {
                _line++;

                string[] columns = nextLine.Split(_delimiter);

                _keyValuePairs.Clear(); // Not strictly required. Could just overwrite as column headers stay the same.

                if ((columns is null) || (columns is null) || (columns.Length != _dtxHeaders.Length))
                {
                    throw new ArgumentException($"Keys/values nust be non-null and contain the same number of elements.");
                }

                for (int i = 0; i < _dtxHeaderIndices.Count; i++)
                {
                    _keyValuePairs[_dtxHeaders[_dtxHeaderIndices[i]]] = columns[_dtxHeaderIndices[i]];
                }

                _dtxRow = (T)DEFAULT_OBJECT.Build(_keyValuePairs);

                return true;
            }

            _enumerationFinished = true;
        }

        // At the end of the enumeration. Close the reader.
        _reader.Close();

        return false;
    }

    public void Reset()
    {
        if (_reader is not null)
        {
            _reader.Reset();
            _line = START_LINE;
            _enumerationFinished = false;
        }
    }
}