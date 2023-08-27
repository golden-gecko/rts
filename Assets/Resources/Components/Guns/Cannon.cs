using UnityEngine;

public class Cannon : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        GameObject gameObject = Instantiate(MissilePrefab, myGameObject.Center, Quaternion.identity);
        Missile missile = gameObject.GetComponent<Missile>();

        missile.Parent = myGameObject;
        missile.Player = myGameObject.Player;

        missile.Move(position);
        missile.Destroy();

        Reload.Reset();
    }
}
