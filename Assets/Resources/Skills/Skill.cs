using System;
using System.Collections.Generic;
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

    public virtual void Update(MyGameObject myGameObject)
    {
        Cooldown.Update(Time.deltaTime);

        RaycastHit[] hitInfos = Utils.SphereCastAll(myGameObject.Position, Range, LayerMask.GetMask("GameObject"));

        foreach (RaycastHit hitInfo in hitInfos)
        {
            MyGameObject target = Utils.GetGameObject(hitInfo);

            if (Timers.ContainsKey(target) == false)
            {
                Timers.Add(target, new Timer());
            }
        }
    }

    public string GetInfo()
    {
        return string.Format("Name: {0}, Cooldown: {1}, Range: {2}", Name, Cooldown.GetInfo(), Range);
    }

    public string Name { get; }

    public float Range { get; } = 0.0f;

    public Timer Cooldown { get; }

    public string Effect { get; protected set; } = string.Empty;

    private Dictionary<MyGameObject, Timer> Timers = new Dictionary<MyGameObject, Timer>();
}
