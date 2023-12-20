using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

/// <summary>
/// Data structure for storing values using a unique enum value as an index.
/// </summary>
/// <typeparam name="T">Stored Type</typeparam>
/// <typeparam name="TE">Enum type</typeparam>
public class EnumSet<T, TE> : IEnumerable<T>
{
    private readonly int _size;
    
    public int Length { get; private set; }

    [ItemCanBeNull] private readonly T[] _values;
    private readonly int[] _keys;
    
    public EnumSet()
    {
        _size = Enum.GetNames(typeof(TE)).Length;
        _values = new T[_size];
        _keys = new int[_size];

        Length = 0;
    }

    public bool SetValue(int key, T value)
    {
        if (key >= _size) throw new ArgumentOutOfRangeException("", "Enum is not long enough");
        if (_values[key] != null) return false;
        
        _keys[Length++] = key;
        _values[key] = value;
        return true;
    }
    
    public bool SetValue(TE index, T value)
    {
        int key = (int)(object)index;
        if (key >= _size) throw new ArgumentOutOfRangeException("", "Enum is not long enough");
        if (_values[key] != null) return false;
        
        _keys[Length++] = key;
        _values[key] = value;
        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new EnumSetEnumerator(this, _values, _keys);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public T this[int key] => _values[key];
    public T this[TE key] => _values[(int)(object)key];
    
    class EnumSetEnumerator : IEnumerator<T>
    {
        private int _index;
        private readonly T[] _values;
        private readonly int[] _keys;
        
        private readonly EnumSet<T,TE> _base;

        public EnumSetEnumerator(EnumSet<T,TE> enumerable, T[] values, int[] keys)
        {
            _base = enumerable;
            _values = values;
            _keys = keys;

            _index = -1;
        }

        public bool MoveNext()
        {
            return ++_index < _base.Length;
        }

        public void Reset()
        {
            _index = -1;
        }

        T IEnumerator<T>.Current => _values[_keys[_index]];

        public object Current => _values[_keys[_index]];

        public void Dispose() { }
    }
}


