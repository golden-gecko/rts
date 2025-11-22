public class Engine : MyComponent
{
    public Engine(MyGameObject parent, string name, float mass, float power) : base(parent, name, mass)
    {
        Power = power;
    }

    public override string GetInfo()
    {
        return string.Format("Name: {0}, Speed: {1:0.}", Name, Speed);
    }

    public float Speed { get => Power / Parent.Mass; }

    public float Power { get; }
}
