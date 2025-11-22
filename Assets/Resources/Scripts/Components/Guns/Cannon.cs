using UnityEngine;

[DisallowMultipleComponent]
public class Cannon : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        if (CanFire() == false)
        {
            return;
        }

        Reload.Reset();

        GameObject gameObject = Instantiate(MissilePrefab, myGameObject.Center, Quaternion.identity);
        Missile missile = gameObject.GetComponent<Missile>();

        missile.Damage = Damage.Clone() as Property;
        missile.Range = Range;
        missile.DamageType = DamageType;

        missile.SetParent(myGameObject);
        missile.SetPlayer(myGameObject.Player);
        missile.Move(position);
        missile.Destroy();

        Ammunition.Dec();

        myGameObject.Stats.Inc(Stats.MissilesFired);
    }
}
