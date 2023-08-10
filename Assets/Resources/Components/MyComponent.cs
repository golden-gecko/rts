public class MyComponent
{
    public MyComponent(MyGameObject parent, string name)
    {
        Parent = parent;
        Name = name;
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
}
