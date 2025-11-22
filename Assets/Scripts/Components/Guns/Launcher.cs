using UnityEngine;

[DisallowMultipleComponent]
public class Launcher : Gun
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
        missile.Move(myGameObject.Position + new Vector3(0.0f, 30.0f, 0.0f));
        missile.Move(position);
        missile.Destroy();

        Ammunition.Dec();

        myGameObject.Stats.Inc(Stats.MissilesFired);
    }

    public override string GetInfo()
    {
        return string.Format("Launcher - {0}", base.GetInfo());
    }
}
