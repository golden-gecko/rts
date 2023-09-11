using System.Linq;

public class Recipe
{
    public Recipe(string name)
    {
        Name = name;
    }

    public void Consumes(string name, int count)
    {
        ToConsume.Init(name, 0, count, ResourceDirection.In);
    }

    public void Produces(string name, int count)
    {
        ToProduce.Init(name, 0, count, ResourceDirection.Out);
    }

    public string Name { get; }

    public int Sum { get => ToConsume.Items.Sum(x => x.Max) + ToProduce.Items.Sum(x => x.Max); }

    public ResourceContainer ToConsume { get; } = new ResourceContainer();

    public ResourceContainer ToProduce { get; } = new ResourceContainer();
}
