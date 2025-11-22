public class Counter
{
    public Counter(int current, int max)
    {
        Current = current;
        Max = max;
    }

    public void Add(int current)
    {
        Current += current;

        if (Current > Max)
        {
            Current = Max;
        }
    }

    public void Remove(int current)
    {
        Current -= current;

        if (Current < 0)
        {
            Current = 0;
        }
    }

    public bool CanAdd(int current)
    {
        return Current + current <= Max;
    }

    public bool CanRemove(int current)
    {
        return Current - current >= 0;
    }

    public void Reset()
    {
        Current = 0;
    }

    public int Current { get; set; }

    public int Max { get; set; }
}
