public class Timer
{
    public Timer(float max)
    {
        Current = 0.0f;
        Max = max;
    }

    public void Update(float time)
    {
        Current += time;

        if (Current > Max)
        {
            Current = Max;
        }
    }

    public void Reset()
    {
        Current = 0.0f;
    }

    public float Current { get; private set; }

    public float Max { get; }

    public bool Finished { get => Current >= Max; }
}
