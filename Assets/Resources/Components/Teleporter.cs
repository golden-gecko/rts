using UnityEngine;

public class Teleporter : MyComponent
{
    protected override void Start()
    {
        base.Start();

        /*
        Parent.Orders.AllowOrder(OrderType.Assemble);
        Parent.Orders.AllowOrder(OrderType.Rally);

        foreach (string prefab in Prefabs)
        {
            Parent.Orders.AllowPrefab(Path.Join(Config.Asset.Units, prefab));
        }

        Parent.OrderHandlers[OrderType.Assemble] = new OrderHandlerAssemble();
        Parent.OrderHandlers[OrderType.Rally] = new OrderHandlerRally();
        */
    }

    protected override void Update()
    {
        base.Update();

        foreach (RaycastHit hitInfo in Utils.SphereCastAll(Parent.Position, Parent.Radius, Utils.GetGameObjectMask()))
        {
        }
    }

    public override string GetInfo()
    {
        return string.Format("Teleporter: {0}", base.GetInfo());
    }
}
