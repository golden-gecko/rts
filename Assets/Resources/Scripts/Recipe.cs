using System.Collections.Generic;

public class Recipe
{
    public void Consume(string name, int count)
    {
        ToConsume.Add(new RecipeComponent(name, count));
    }

    public void Produce(string name, int count)
    {
        ToProduce.Add(new RecipeComponent(name, count));
    }

    public List<RecipeComponent> ToConsume { get; } = new List<RecipeComponent>();

    public List<RecipeComponent> ToProduce { get; } = new List<RecipeComponent>();
}
