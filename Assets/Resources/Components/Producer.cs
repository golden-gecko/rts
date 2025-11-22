using UnityEngine;

public class Producer : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1;

    public RecipeContainer Recipes { get; } = new RecipeContainer();
}
