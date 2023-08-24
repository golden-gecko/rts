using UnityEngine;

public class Cannon : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        Missile missile = Instantiate(MissilePrefab, myGameObject.Center, Quaternion.identity).GetComponent<Missile>();

        missile.Parent = myGameObject;
        missile.Player = myGameObject.Player;

        missile.Move(position);
        missile.Destroy();

        Reload.Reset();
    }
}
