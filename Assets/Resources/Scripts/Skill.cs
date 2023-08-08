using UnityEngine;

public class Skill : MonoBehaviour
{
    public Skill(string name)
    {
        Name = name;
    }

    public virtual void Execute()
    {
    }

    public string GetInfo()
    {
        return name;
    }

    public string Name { get; }
}
