public class unit_Quad_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Speed = 12.0f;
        MissilePrefab = "Prefabs/Missiles/Missile";
        MissileRangeMax = 20.0f;
        ReloadTimer = new Timer(3.0f);
    }
}
