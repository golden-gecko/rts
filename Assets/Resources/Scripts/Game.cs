using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Order CreateAttackJob(MyGameObject myGameObject)
    {
        float minDistance = float.MaxValue;
        MyGameObject closest = null;

        foreach (MyGameObject target in FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (myGameObject.Is(target, DiplomacyState.Enemy) == false)
            {
                continue;
            }

            Gun gun = myGameObject.GetComponent<Gun>();

            if (gun == null)
            {
                continue;
            }

            if (gun.IsInRange(target.Position) == false)
            {
                continue;
            }

            if (target.GetComponent<Missile>())
            {
                continue;
            }

            float distance = myGameObject.DistanceTo(target);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target;
            }
        }

        if (closest != null)
        {
            return Order.Attack(closest);
        }

        return null;
    }
}
