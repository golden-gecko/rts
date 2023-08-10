public class Engine
{
    public Engine(string name, float speed)
    {
        Name = name;
        Speed = speed;
    }

    public virtual void Update()
    {
    }

    public virtual string GetInfo()
    {
        return string.Format("Name: {0}, Speed: {0:0.}", Name, Speed);
    }

    public string Name { get; }

    public float Speed { get; }
}
