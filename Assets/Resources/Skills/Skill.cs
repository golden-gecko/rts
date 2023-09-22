using System;
using UnityEngine;

public class Skill : ICloneable
{
    public virtual object Clone()
    {
        return new Skill(Name, Range, Cooldown.Max, Effect, Passive);
    }

    public Skill(string name, float range, float cooldown, string effect, bool passive)
    {
        Name = name;
        Range = range;
        Cooldown = new Timer(cooldown, cooldown);
        Effect = effect;
        Passive = passive;
    }

    public virtual void Update(MyGameObject myGameObject)
    {
        Cooldown.Update(Time.deltaTime);
    }

    public virtual void OnDestroy(MyGameObject myGameObject)
    {
    }

    public virtual void OnExecute(MyGameObject myGameObject)
    {
        Cooldown.Reset();
    }

    public virtual void OnMove(MyGameObject myGameObject, Vector3 position)
    {
    }

    public string GetInfo()
    {
        return string.Format("Name: {0}, Range: {1}, Cooldown: {2}, ", Name, Range, Cooldown.GetInfo());
    }

    public string Name { get; }

    public float Range { get; }

    public Timer Cooldown { get; }

    public string Effect { get; }

    public bool Passive { get; }
}
