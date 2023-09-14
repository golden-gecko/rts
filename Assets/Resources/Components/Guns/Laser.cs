using UnityEngine;

public class Laser : Gun
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

        missile.Parent = myGameObject;
        missile.Player = myGameObject.Player;
        missile.Damage = Damage;
        missile.Range = Range;

        missile.Attack(position);

        Ammunition.Dec();

        myGameObject.Stats.Inc(Stats.MissilesFired);
    }
}
