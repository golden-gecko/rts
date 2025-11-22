using UnityEngine;

public class Cannon : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        MyGameObject resource = Resources.Load<MyGameObject>(MissilePrefab);
        MyGameObject missile = Object.Instantiate(resource, myGameObject.Center, Quaternion.identity);

        missile.Parent = myGameObject;
        missile.Player = myGameObject.Player;

        missile.Move(position);
        missile.Destroy();

        Reload.Reset();
    }
}
