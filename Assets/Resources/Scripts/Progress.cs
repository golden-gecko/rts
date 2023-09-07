using System;
using UnityEngine;

[Serializable]
public class Progress
{
    public Progress(float current = 0.0f, float max = 0.0f)
    {
        Current = current;
        Max = max;
    }

    public void Add(float current)
    {
        Current += current;

        if (Current > Max)
        {
            Current = Max;
        }
    }

    public void Remove(float current)
    {
        Current -= current;

        if (Current < 0.0f)
        {
            Current = 0.0f;
        }
    }

    public void Inc()
    {
        Add(1.0f);
    }

    public void Dec()
    {
        Remove(1.0f);
    }

    public bool CanAdd(float current)
    {
        return Current + current <= Max;
    }

    public bool CanRemove(float current)
    {
        return Current - current >= 0.0f;
    }

    public string GetInfo()
    {
        return string.Format("{0:0.}/{1:0.}", Current, Max);
    }

    [field: SerializeField]
    public float Current { get; private set; }

    [field: SerializeField]
    public float Max { get; private set; }
}
