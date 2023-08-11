using UnityEngine;

public class Researcher : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Research Time: {1:0.}", base.GetInfo(), ResearchTime);
    }

    [field: SerializeField]
    public float ResearchTime { get; set; } = 2.0f; // TODO: Replace with rate (resource per second).
}
