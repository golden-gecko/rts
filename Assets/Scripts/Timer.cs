public class Timer
{
    public Timer()
    {
        Value = 0.0f;
        Max = 0.0f;
    }

    public Timer(float max)
    {
        Value = 0.0f;
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
        Value = 0.0f;
    }

    public bool Finished { get => Value >= Max; }

    public float Value { get; private set; }

    public float Max { get; }
}
