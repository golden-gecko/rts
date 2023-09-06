using UnityEngine;

public class PowerPlant : MyComponent
{
    protected override void Update()
    {
        base.Update();

        MyGameObject parent = GetComponent<MyGameObject>();

        if (parent.Working == false)
        {
            return;
        }

        if (Power > 0.0f || parent.Powered)
        {
            // Map.Instance.SetVisibleByPower(parent, Range);
        }
        else
        {
            // Map.Instance.UnsetVisibleByPower(parent, Range);
        }
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Power: {1}, Range: {2}", base.GetInfo(), Power, Range);
    }

    [field: SerializeField]
    public float Power { get; set; } = 20.0f;

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;
}
