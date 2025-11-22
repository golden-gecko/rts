using UnityEngine;

public class struct_Refinery_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Produce);

        Resources.Add("Metal Ore", new Resource("Metal Ore", 100));
        Resources.Add("Metal", new Resource("Metal", 0));
    }

    protected override void OnIdleOrder()
    {
        Produce();

        Orders.Pop();
    }
}
