using UnityEngine;

public class Cannon : Gun
{
    public Cannon(MyGameObject parent, string name, float mass, float damage, float range, float reload) : base(parent, name, mass, damage, range, reload)
    {
    }

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
