public class Turret_Missile : Turret
{
    protected override void Awake()
    {
        base.Awake();

        Gun = new Cannon("Cannon", 10.0f, 20.0f, 2.0f);
        Gun.MissilePrefab = "Objects/Missiles/Rocket";
    }
}
