using UnityEngine;

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

        missile.Parent = myGameObject;
        missile.Player = myGameObject.Player;

        missile.Move(position);
        missile.Destroy();

        GetComponent<Storage>().Resources.Dec("Ammunition");

        myGameObject.Stats.Inc(Stats.MissilesFired);
    }
}
