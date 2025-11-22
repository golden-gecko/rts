using UnityEngine;

public class Engine : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Explore);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Follow);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Guard);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Move);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Patrol);

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Explore] = new OrderHandlerExplore();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Follow] = new OrderHandlerFollow();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Guard] = new OrderHandlerGuard();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Move] = new OrderHandlerMove();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Patrol] = new OrderHandlerPatrol();
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Power: {1:0.}, Speed: {2:0.}", base.GetInfo(), Power, Speed);
    }

    [field: SerializeField]
    public float Power { get; set; } = 50.0f;

    public float Speed { get => Power / GetComponent<MyGameObject>().Mass; }
}
