using System.Collections.Generic;
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

        DisasterTime.Max = DisasterFrequencyInSeconds;
    }

    protected void Start()
    {
        Player cpu = GameObject.Find("CPU").GetComponent<Player>();
        Player gaia = GameObject.Find("Gaia").GetComponent<Player>();
        Player human = GameObject.Find("Human").GetComponent<Player>();

        cpu.SetDiplomacy(cpu, DiplomacyState.Ally);
        cpu.SetDiplomacy(gaia, DiplomacyState.Neutral);
        cpu.SetDiplomacy(human, DiplomacyState.Enemy);

        gaia.SetDiplomacy(cpu, DiplomacyState.Neutral);
        gaia.SetDiplomacy(gaia, DiplomacyState.Neutral);
        gaia.SetDiplomacy(human, DiplomacyState.Neutral);

        human.SetDiplomacy(cpu, DiplomacyState.Enemy);
        human.SetDiplomacy(gaia, DiplomacyState.Neutral);
        human.SetDiplomacy(human, DiplomacyState.Ally);
    }

    protected void Update()
    {
        CreateDisaster();
    }

    public Order CreataAttackJob(MyGameObject myGameObject)
    {
        float minDistance = float.MaxValue;
        MyGameObject closest = null;

        foreach (MyGameObject target in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (myGameObject.Is(target, DiplomacyState.Enemy) == false)
            {
                continue;
            }

            if (myGameObject.IsInAttackRange(target.Position) == false)
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

    public MyGameObject CreateGameObject(string prefab, Vector3 position, Player player, MyGameObjectState state)
    {
        MyGameObject resource = Resources.Load<MyGameObject>(prefab);
        MyGameObject myGameObject = Object.Instantiate<MyGameObject>(resource, position, Quaternion.identity);

        myGameObject.SetPlayer(player);
        myGameObject.State = state;

        if (state == MyGameObjectState.Cursor)
        {
            foreach (Collider collider in myGameObject.GetComponents<Collider>())
            {
                collider.enabled = false;
            }

            foreach (MyComponent myComponent in myGameObject.GetComponents<MyComponent>())
            {
                myComponent.enabled = false;
            }

            foreach (MyGameObject i in myGameObject.GetComponents<MyGameObject>())
            {
                i.enabled = false;
            }
        }

        return myGameObject;
    }

    private void CreateDisaster()
    {
        /*if (DisasterTime.Update(Time.deltaTime))
        {
            DisasterTime.Reset();

            // TODO: Put this somewhere.
            List<string> _disasters = new List<string>()
            {
                "Objects/Disasters/Fire",
                "Objects/Disasters/Meteor",
                "Objects/Disasters/Tornado",
            };

            int index = Random.Range(0, _disasters.Count);

            CreateGameObject(_disasters[1], new Vector3(300.41f, 0.0f, 132.32f), GetGaiaPlayer(), MyGameObjectState.Operational);
        }*/
    }

    private Player GetGaiaPlayer()
    {
        return GameObject.Find("Gaia").GetComponent<Player>();
    }

    [field: SerializeField]
    public float DisasterFrequencyInSeconds = 120.0f;

    private Timer DisasterTime = new Timer();
}
