using System.Collections.Generic;

public class RecipeComponent
{
    public RecipeComponent(string name, int count)
    {
        Name = name;
        Count = count;
    }

    public string Name { get; }

    public int Count { get; }
}

public class Recipe
{
    public Recipe()
    {
        ToConsume = new List<RecipeComponent>();
        ToProduce = new List<RecipeComponent>();
    }

    // TODO: Implemented count.
    public void Consume(string name, int count)
    {
        ToConsume.Add(new RecipeComponent(name, count));
    }

    // TODO: Implemented count.
    public void Produce(string name, int count)
    {
        ToProduce.Add(new RecipeComponent(name, count));
    }

    public List<RecipeComponent> ToConsume { get; }
    public List<RecipeComponent> ToProduce { get; }
}
