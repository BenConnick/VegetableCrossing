using System.Collections;
using System.Collections.Generic;

public class IntSet<T> : IEnumerable<T>
{
    private readonly List<T> valueList = new List<T>();
    private readonly List<bool> isSetList = new List<bool>();

    public void Set(int index, T entry)
    {
        if (index < 0)
        {
            throw new System.IndexOutOfRangeException($"IntSet exception: (index {index}) You cannot create an entry in an IntSet with an index < 0");
        }
        if (index >= valueList.Count)
        {
            int count = valueList.Count;
            for (int i = 0; i < 1 + index - count; i++)
            {
                valueList.Add(default(T));
                isSetList.Add(false);
            }
        }
        valueList[index] = entry;
        isSetList[index] = true;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= valueList.Count) throw new System.IndexOutOfRangeException($"No entry in the set exists for index {index}");
        return valueList[index];
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= valueList.Count) throw new System.IndexOutOfRangeException($"No entry in the set exists for index {index}");
        valueList[index] = default(T);
        isSetList[index] = false;
    }

    public bool IsSet(int index)
    {
        // operator == is undefined for generic T; EqualityComparer solves this
        // "equals default" is the same as unset, so if you are setting default values, this happens
        return index > 0 && index < valueList.Count && isSetList[index];
    }

    private int GetNext(int startingWith)
    {
        for (int i = startingWith+1; i < isSetList.Count; i++)
        {
            if (isSetList[i]) return i;
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
        private int index = -1;
        private IntSet<T2> reference;

        public IntSetEnumerator(IntSet<T2> collection) {
            reference = collection;
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
            index = reference.GetNext(index);
            return index >= 0;
        }

        public void Reset()
        {
            index = reference.GetNext(-1);
        }
    }
}
