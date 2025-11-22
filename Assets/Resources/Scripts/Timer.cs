public class Timer
{
    public Timer(float max = 0.0f)
    {
        Current = 0.0f;
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

    public float Current { get; private set; }

    public float Max { get; set; }

    public bool Finished { get => Current >= Max; }
}
