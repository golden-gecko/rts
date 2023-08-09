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

    public string GetInfo()
    {
        return Name;
    }

    public string Name { get; }

    public Timer Cooldown { get; } = new Timer();

    public float Range { get; } = 0.0f;
}
