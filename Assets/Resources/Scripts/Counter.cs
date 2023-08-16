public class Counter
{
    public Counter(int current = 0, int max = 0)
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

    public void Inc()
    {
        Add(1);
    }

    public void Dec()
    {
        Remove(1);
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

    public int Current { get; private set; }

    public int Max { get; set; } // TODO: Hide setter.
}
