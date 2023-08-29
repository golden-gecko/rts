using System.Linq;

public class Recipe
{
    public Recipe(string name)
    {
        Name = name;
    }

    public void Consumes(string name, int count)
    {
        ToConsume.Add(name, 0, count);
    }

    public void Produces(string name, int count)
    {
        ToProduce.Add(name, 0, count);
    }

    public string Name { get; }

    public int Sum
    {
        get
        {
            return ToConsume.Items.Values.Sum(x => x.Max) + ToProduce.Items.Values.Sum(x => x.Max);
        }
    }

    public ResourceContainer ToConsume { get; } = new ResourceContainer();

    public ResourceContainer ToProduce { get; } = new ResourceContainer();
}
