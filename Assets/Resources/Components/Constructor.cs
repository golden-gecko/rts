using UnityEngine;

public class Constructor : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Construct);
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 2;
}
