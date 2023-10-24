using UnityEngine;

[DisallowMultipleComponent]
public class Bullet : Missile
{
    protected override void OnCollisionEnter(Collision collision)
    {
        MyGameObject myGameObject = Utils.GetGameObject(collision);

        if (myGameObject != null)
        {
            Collide(myGameObject);
        }
        else if (collision.collider.TryGetComponent(out Terrain _))
        {
            if (collision.contacts.Length > 0)
            {
                Collide(collision.contacts[0].point);
            }
        }
    }

    private void Collide(MyGameObject myGameObject)
    {
        if (myGameObject.Is(this, DiplomacyState.Ally))
        {
            return;
        }

        if (myGameObject.GetComponent<Missile>())
        {
            return;
        }

        if (Map.Instance.IsVisibleBySight(myGameObject.Position, HUD.Instance.ActivePlayer))
        {
            Instantiate(HitEffectPrefab, Position, Quaternion.identity);
        }

        float damageDealt = myGameObject.OnDamageHandler(DamageType, Damage.Total);

        if (myGameObject.Alive == false)
        {
            Parent.Stats.Inc(Stats.TargetsDestroyed);
        }

        Parent.Stats.Add(Stats.DamageDealt, damageDealt);

        Destroy(0);
    }

    private void Collide(Vector3 position)
    {
        if (Map.Instance.IsVisibleBySight(position, HUD.Instance.ActivePlayer))
        {
            Instantiate(HitEffectPrefab, Position, Quaternion.identity);
        }

        Destroy(0);
    }
}
