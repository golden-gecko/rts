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
            int sum = 0;

            foreach (Resource i in ToConsume.Items.Values)
            {
                sum += i.Max;
            }

            foreach (Resource i in ToProduce.Items.Values)
            {
                sum += i.Max;
            }

            return sum;
        }
    }

    public ResourceContainer ToConsume { get; } = new ResourceContainer();

    public ResourceContainer ToProduce { get; } = new ResourceContainer();
}
