using UnityEngine;

public class Laser : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        if (CanFire() == false)
        {
            myGameObject.Wait(0);

            return;
        }

        GameObject gameObject = Instantiate(MissilePrefab, myGameObject.Center, Quaternion.identity);
        Missile missile = gameObject.GetComponent<Missile>();

        missile.Parent = myGameObject;
        missile.Player = myGameObject.Player;

        missile.Target = position; // TODO: Replace with order.
        missile.Range = Range;

        missile.Wait();
        missile.Destroy();

        Ammunition -= 1;

        myGameObject.Stats.Inc(Stats.MissilesFired);

        Reload.Reset();
    }
}
