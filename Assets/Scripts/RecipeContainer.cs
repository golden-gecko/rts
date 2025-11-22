using System.Collections;
using System.Collections.Generic;

public class RecipeContainer : IEnumerable<Recipe>
{
    public RecipeContainer()
    {
        Items = new List<Recipe>();
    }

    public void Add(Recipe recipe)
    {
        Items.Add(recipe);
    }

    public IEnumerator<Recipe> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public List<Recipe> Items { get; }
}
