using UnityEngine;

public class Laser : Gun
{
    public Laser(string name, float damage, float range, float reload) : base(name, damage, range, reload)
    {
    }

    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        Reload.Reset();
    }
}
