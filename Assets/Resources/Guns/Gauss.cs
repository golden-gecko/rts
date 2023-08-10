using UnityEngine;

public class Gauss : Gun
{
    public Gauss(string name, float damage, float range, float reload) : base(name, damage, range, reload)
    {
    }

    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        Reload.Reset();
    }
}
