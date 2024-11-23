using System.Collections.ObjectModel;
using System;

namespace Krystal.Types;

/// <summary>
/// Represents a flat 1D array as a 2-dimensional array.
/// </summary>
/// <typeparam name="T">Type</typeparam>
public class Array2D<T>
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
    
    // Optional: Get the dimensions of the array
    public int SizeX => _sizeX;
    public int SizeY => _sizeY;
}