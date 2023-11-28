using UnityEngine;

[DisallowMultipleComponent]
public class Gauss : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        if (CanFire() == false)
        {
            return;
        }

        Reload.Reset();

        GameObject gameObject = Instantiate(MissilePrefab, myGameObject.GetComponentInChildren<Gun>().Center, Quaternion.identity);
        Missile missile = gameObject.GetComponent<Missile>();

        missile.Damage = Damage.Clone() as Property;
        missile.Range = Range;
        missile.DamageType = DamageType;

        missile.SetParent(myGameObject);
        missile.SetPlayer(myGameObject.Player);
        missile.AttackPosition(position);
        missile.transform.LookAt(position);
        missile.transform.localScale = new Vector3(1.0f, 1.0f, (myGameObject.GetComponentInChildren<Gun>().Center - position).magnitude);

        Ammunition.Dec();

        myGameObject.Stats.Inc(Stats.MissilesFired);

        if (AudioFire != null)
        {
            AudioSource.PlayClipAtPoint(AudioFire, Parent.Position, 1.0f);
        }
    }

    public override string GetInfo()
    {
        return string.Format("Gauss - {0}", base.GetInfo());
    }
}
