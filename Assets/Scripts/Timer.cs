public class Timer
{
    public Timer()
    {
        Value = 0;
        Max = 0;
    }

    public Timer(float max)
    {
        Value = 0;
        Max = max;
    }

    public void Update(float time)
    {
        Value += time;

        if (Value > Max)
        {
            Value = Max;
        }
    }

    public void Reset()
    {
        Value = 0;
    }

    public bool Finished { get => Value >= Max; }

    public float Value { get; private set; }

    public float Max { get; private set; }
}
