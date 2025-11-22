using System;

public class Achievement
{
    public Achievement(string name, string description, Func<bool> handler)
    {
        Name = name;
        Description = description;
        Handler = handler;
    }

    public bool Check()
    {
        return Completed = Handler();
    }

    public string Name { get; }

    public string Description { get; }

    public bool Completed { get; private set; } = false;

    private Func<bool> Handler;
}
