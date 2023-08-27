using UnityEngine;

public class Gauss : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        GameObject gameObject = Instantiate(MissilePrefab, myGameObject.Center, Quaternion.identity);
        Missile missile = gameObject.GetComponent<Missile>();

        missile.Parent = myGameObject;
        missile.Player = myGameObject.Player;

        missile.Target = position; // TODO: ???
        missile.Range = Range;

        missile.Wait(0.2f); // TODO: Hardcoded.
        missile.Destroy();

        Reload.Reset();
    }
}
