public class Skill
{
    public Skill(string name)
    {
        Name = name;
    }

    public virtual void Execute(MyGameObject myGameObject)
    {
    }

    public string GetInfo()
    {
        return Name;
    }

    public string Name { get; }
}
