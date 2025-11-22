public class Engine : MyComponent
{
    public Engine(MyGameObject parent, string name, float speed) : base(parent, name)
    {
        Speed = speed;
    }

    public override string GetInfo()
    {
        return string.Format("Name: {0}, Speed: {1:0.}", Name, Speed);
    }

    public float Speed { get; }
}
