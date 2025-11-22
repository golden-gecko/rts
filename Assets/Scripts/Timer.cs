public class Timer
{
    public Timer(float max)
    {
        Value = 0.0f;
        Max = max;
    }

    public void Reset()
    {
        Value = 0.0f;
    }

    public void Update(float time)
    {
        Value += time;

        if (Value > Max)
        {
            Value = Max;
        }
    }

    public bool Finished { get => Value >= Max; }

    public float Value { get; private set; }

    public float Max { get; }
}
