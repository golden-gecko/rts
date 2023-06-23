public class unit_Grav_Light_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Speed = 8;
        MissilePrefab = "Missiles/Missile";
        MissileRangeMax = 30;
        ReloadTimer = new Timer(10.0f);
    }
}
