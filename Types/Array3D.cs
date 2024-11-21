using System.Collections.ObjectModel;
using System;

namespace Krystal.Types;

public class Array3D<T>
{
    private readonly T[] _data;
    private readonly int _sizeX, _sizeY, _sizeZ;

    public ReadOnlyCollection<T> Data => Array.AsReadOnly(_data);

    public Array3D(int sizeX, int sizeY, int sizeZ)
    {
        this._sizeX = sizeX;
        this._sizeY = sizeY;
        this._sizeZ = sizeZ;
        _data = new T[sizeX * sizeY * sizeZ];
    }
    
    public T this[int x, int y, int z]
    {
        get
        {
            CheckBounds(x, y, z);
            return _data[GetIndex(x, y, z)];
        }
        set
        {
            CheckBounds(x, y, z);
            _data[GetIndex(x, y, z)] = value;
        }
    }
    
    private int GetIndex(int x, int y, int z)
    {
        return x * (_sizeY * _sizeZ) + y * _sizeZ + z;
    }

    public void SetAllValues(T value)
    {
        for (var i = 0; i < _data.Length; i++)
            _data[i] = value;
    }
    
    public (int x, int y, int z) GetPosition(int index)
    {
        if (index < 0 || index >= _data.Length)
            throw new IndexOutOfRangeException("Index out of bounds");

        // Calculate x, y, z
        int x = index / (_sizeY * _sizeZ);
        int remainderXy = index % (_sizeY * _sizeZ);
        int y = remainderXy / _sizeZ;
        int z = remainderXy % _sizeZ;

        return (x, y, z);
    }
    
    private void CheckBounds(int x, int y, int z)
    {
        if (x < 0 || x >= _sizeX || y < 0 || y >= _sizeY || z < 0 || z >= _sizeZ)
            throw new IndexOutOfRangeException($"Indices out of range: ({x}, {y}, {z})");
    }
    
    public int SizeX => _sizeX;
    public int SizeY => _sizeY;
    public int SizeZ => _sizeZ;
}
