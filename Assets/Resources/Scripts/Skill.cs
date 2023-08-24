using UnityEngine;

public class Skill
{
    public Skill(string name, float cooldown, float range)
    {
        Name = name;
        Cooldown.Max = cooldown;
        Range = range;
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

    public Timer Cooldown { get; } = new Timer();

    public float Range { get; } = 0.0f;

    public string Effect { get; protected set; } = string.Empty;
}
