using System;
using UnityEngine;

public class Skill : ICloneable
{
    public virtual object Clone()
    {
        return new Skill(Name, Range, Cooldown.Max, Effect);
    }

    public Skill(string name, float range, float cooldown, string effect)
    {
        Name = name;
        Range = range;
        Cooldown = new Timer(cooldown, cooldown);
        Effect = effect;
    }

    public virtual void Execute(MyGameObject myGameObject)
    {
    }

    public virtual void Update()
    {
        Cooldown.Update(Time.deltaTime);
    }

    public string GetInfo()
    {
        return string.Format("Name: {0}, Cooldown: {1}, Range: {2}", Name, Cooldown.GetInfo(), Range);
    }

    public string Name { get; }

    public float Range { get; } = 0.0f;

    public Timer Cooldown { get; }

    public string Effect { get; protected set; } = string.Empty;
}
