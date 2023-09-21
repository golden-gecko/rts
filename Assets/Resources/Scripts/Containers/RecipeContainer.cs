using System.Collections.Generic;

public class RecipeContainer
{
    public void Add(Recipe recipe)
    {
        Items[recipe.Name] = recipe;
    }

    public Recipe Get(string name)
    {
        return Items[name];
    }

    public Dictionary<string, Recipe> Items { get; } = new Dictionary<string, Recipe>();
}
