using UnityEngine;

public class Gauss : Gun
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

        missile.Damage = Damage;
        missile.DamageFactor = DamageFactor;
        missile.Range = Range;

        missile.SetParent(myGameObject);
        missile.SetPlayer(myGameObject.Player);
        missile.Attack(position);

        Ammunition.Dec();

        myGameObject.Stats.Inc(Stats.MissilesFired);
    }
}
