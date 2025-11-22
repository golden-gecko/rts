using System;
using UnityEngine;

[Serializable]
public class Timer
{
    public Timer(float max = 0.0f)
    {
        Current = 0.0f;
        Max = max;
    }

    public Timer(float current, float max)
    {
        Current = current;
        Max = max;
    }

    public bool Update(float time)
    {
        Current += time;

        if (Current > Max)
        {
            Current = Max;
        }

        return Finished;
    }

    public void Reset()
    {
        Current = 0.0f;
    }

    public string GetInfo()
    {
        return string.Format("{0:0.}/{1:0.}", Current, Max);
    }

    [field: SerializeField]
    public float Current { get; private set; }

    [field: SerializeField]
    public float Max { get; set; }

    public bool Finished { get => Current >= Max; }
}
