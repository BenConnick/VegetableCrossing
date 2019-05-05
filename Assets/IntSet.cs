using System;
using System.Collections;
using System.Collections.Generic;

public class IntSet<T> : IEnumerable<T>
{
    private readonly List<T> values = new List<T>();
    private readonly List<bool> isSet = new List<bool>();

    public void Add(int index, T entry)
    {
        if (index < 0)
        {
            throw new System.IndexOutOfRangeException($"IntSet exception: (index {index}) You cannot create an entry in an IntSet with an index < 0");
        }
        if (index >= values.Count)
        {
            for (int i = 0; i < 1 + index - values.Count; i++)
            {
                values.Add(default(T));
                isSet.Add(false);
            }
        }
        values[index] = entry;
        isSet[index] = true;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= values.Count) throw new System.IndexOutOfRangeException($"No entry in the set exists for index {index}");
        return values[index];
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= values.Count) throw new System.IndexOutOfRangeException($"No entry in the set exists for index {index}");
        values[index] = default(T);
    }

    public bool IsSet(int index)
    {
        // operator == is undefined for generic T; EqualityComparer solves this
        // "equals default" is the same as unset, so if you are setting default values, this happens
        return index > 0 && index < values.Count && isSet[index];
    }

    public int GetNextSet(int startingWith)
    {
        for (int i = startingWith; i < isSet.Count; i++)
        {
            if (isSet[i]) return i;
        }
        return -1;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new IntSetEnumerator<T>(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class IntSetEnumerator<T2> : IEnumerator<T2>
    {
        private int index;
        private IntSet<T2> reference;

        public IntSetEnumerator(IntSet<T2> collection) {
            reference = collection;
            index = reference.GetNextSet(0);
        }

        public T2 Current
        {
            get
            {
                return reference.Get(index);
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
            // not needed?
        }

        public bool MoveNext()
        {
            index = reference.GetNextSet(index);
            return index >= 0;
        }

        public void Reset()
        {
            index = reference.GetNextSet(0);
        }
    }
}
