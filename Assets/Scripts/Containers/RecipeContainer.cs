using System.Collections.Generic;

public class RecipeContainer
{
    public void Add(Recipe recipe)
    {
        Recipes[recipe.Name] = recipe;
    }

    public Recipe Get(string name)
    {
        if (Recipes.TryGetValue(name, out Recipe recipe))
        {
            return recipe;
        }

        return null;
    }

    public Dictionary<string, Recipe> Recipes { get; } = new Dictionary<string, Recipe>();
}
