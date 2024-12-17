using System.Collections.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Krystal.Types;

/// <summary>
/// Represents a flat 1D array as a 2-dimensional array.
/// </summary>
/// <typeparam name="T">Type</typeparam>
public class Array2D<T> : IEnumerable<T>
{
    private readonly T[] _data;
    private readonly int _sizeX, _sizeY;
    
    public Array2D(int sizeX, int sizeY)
    {
        this._sizeX = sizeX;
        this._sizeY = sizeY;
        _data = new T[sizeX * sizeY];
    }
    
    public T this[int x, int y]
    {
        get
        {
            CheckBounds(x, y);
            return _data[GetIndex(x, y)];
        }
        set
        {
            CheckBounds(x, y);
            _data[GetIndex(x, y)] = value;
        }
    }
    
    public ReadOnlyCollection<T> Data => Array.AsReadOnly(_data);
    
    private int GetIndex(int x, int y)
    {
        return x * _sizeY + y;
    }
    
    private void CheckBounds(int x, int y)
    {
        if (x < 0 || x >= _sizeX || y < 0 || y >= _sizeY)
            throw new IndexOutOfRangeException($"Indices out of range: ({x}, {y})");
    }

    public void SetAllValues(T value)
    {
        for (int i = 0; i < _data.Length; i++)
            _data[i] = value;
    }

    public int SizeX => _sizeX;
    public int SizeY => _sizeY;
    
    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)_data).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}