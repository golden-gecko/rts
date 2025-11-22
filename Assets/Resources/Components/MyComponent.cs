public class MyComponent
{
    public MyComponent(MyGameObject parent, string name, float mass)
    {
        Parent = parent;
        Name = name;
        Mass = mass;
    }

    public virtual void Update()
    {
    }

    public virtual string GetInfo()
    {
        return string.Format("Name: {0}", Name);
    }

    public MyGameObject Parent { get; }

    public string Name { get; }

    public float Mass { get; }
}
