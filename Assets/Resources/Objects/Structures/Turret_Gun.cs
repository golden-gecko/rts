public class Turret_Gun : Turret
{
    protected override void Awake()
    {
        base.Awake();

        Gun = new Laser(this, "Laser", 10.0f, 20.0f, 2.0f);
        Gun.MissilePrefab = "Objects/Missiles/Rocket";
    }
}
