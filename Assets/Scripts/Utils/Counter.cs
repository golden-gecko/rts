using System;
using UnityEngine;

[Serializable]
public class Counter
{
    public Counter(int current = 0, int max = 0)
    {
        Current = current;
        Max = max;
    }

    public int Add(int value)
    {
        int valueToAdd = Mathf.Min(Max - Current, value);

        Current = Mathf.Clamp(Current + value, 0, Max);

        return valueToAdd;
    }

    public int Remove(int value)
    {
        int valueToRemove = Mathf.Min(Current, value);

        Current = Mathf.Clamp(Current - value, 0, Max);

        return valueToRemove;
    }

    public void RemoveAll()
    {
        Current = 0;
    }

    public void Inc()
    {
        Add(1);
    }

    public void Dec()
    {
        Remove(1);
    }

    public bool CanAdd(int value)
    {
        return Current + value <= Max;
    }

    public bool CanRemove(int value)
    {
        return Current - value >= 0;
    }

    public bool CanInc()
    {
        return CanAdd(1);
    }

    public bool CanDec()
    {
        return CanRemove(1);
    }

    public string GetInfo()
    {
        return string.Format("{0}/{1}", Current, Max);
    }

    [field: SerializeField, Range(0, 1000)]
    public int Current { get; private set; }

    [field: SerializeField, Range(0, 1000)]
    public int Max { get; private set; }

    public float Percent { get => (float)Current / (float)Max; }
}
