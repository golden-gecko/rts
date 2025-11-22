using System.Collections.Generic;
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
        r2.Consumes("Coal", 2);
        r2.Consumes("Iron Ore", 1);
        // =>
        r2.Produces("Iron", 1);

        Recipe r3 = new Recipe("Fuel");
        // <=
        r1.Consumes("Crude Oil", 1);
        // =>
        r1.Produces("Fuel", 1);

        Recipes.Add(r1);
        Recipes.Add(r2);
        Recipes.Add(r3);
    }

    private RecipeContainer Recipes = new RecipeContainer();
}
