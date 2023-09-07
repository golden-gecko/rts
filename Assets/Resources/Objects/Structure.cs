using UnityEngine;

public class Structure : MyGameObject
{
    public override string GetInfo(bool ally)
    {
        return string.Format("{0}\nPower Usage: {1:0.}", base.GetInfo(ally), PowerUsage);
    }

    [field: SerializeField]
    public float PowerUsage { get; set; } = 1.0f; // TODO: Implement.
}
