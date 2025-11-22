using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    void Awake()
    {
        CreateRecipes();
    }

    public Recipe Get(string name)
    {
        return Recipes.Get(name);
    }

    private void CreateRecipes()
    {
        Recipe r1 = new Recipe("Charcoal");
        // <=
        r1.Consumes("Wood", 2);
        // =>
        r1.Produces("Coal", 1);

        Recipe r2 = new Recipe("Iron");
        // <=
        r2.Consumes("Coal", 4);
        r2.Consumes("Iron Ore", 2);
        // =>
        r2.Produces("Iron", 1);

        Recipe r3 = new Recipe("Fuel");
        // <=
        r3.Consumes("Oil", 2);
        // =>
        r3.Produces("Fuel", 1);

        Recipe r4 = new Recipe("Plastic");
        // <=
        r4.Consumes("Oil", 2);
        // =>
        r4.Produces("Part", 1);

        Recipes.Add(r1);
        Recipes.Add(r2);
        Recipes.Add(r3);
        Recipes.Add(r4);
    }

    public RecipeContainer Recipes { get; } = new RecipeContainer();
}
