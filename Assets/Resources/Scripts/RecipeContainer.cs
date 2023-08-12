using System.Collections.Generic;

public class RecipeContainer
{
    public void Add(Recipe recipe)
    {
        Items[recipe.Name] = recipe;
    }

    public void Reset(string name)
    {
        if (Items.ContainsKey(name))
        {
            Items[name].Reset();
        }
    }

    public Dictionary<string, Recipe> Items { get; } = new Dictionary<string, Recipe>();
}
