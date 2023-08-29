using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance { get; private set; }

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Recipe r1 = new Recipe("Charcoal");
        Recipe r2 = new Recipe("Iron");

        // <=
        r1.Consumes("Wood", 2);
        // =>
        r1.Produces("Coal", 1);

        // <=
        r2.Consumes("Coal", 2);
        r2.Consumes("Iron Ore", 1);
        // =>
        r2.Produces("Iron", 1);

        Recipes.Add(r1);
        Recipes.Add(r2);
    }

    public Recipe Get(string name)
    {
        return Recipes.Get(name);
    }

    private RecipeContainer Recipes = new RecipeContainer();
}
