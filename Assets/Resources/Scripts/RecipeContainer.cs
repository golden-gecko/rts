using System.Collections.Generic;

public class RecipeContainer
{
    public void Add(Recipe recipe)
    {
        Items.Add(recipe);
    }

    public List<Recipe> Items { get; } = new List<Recipe>();
}
