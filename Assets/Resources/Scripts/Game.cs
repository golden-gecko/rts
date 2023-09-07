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

        Disaster[] disasters = Resources.LoadAll<Disaster>(Config.DirectoryDisasters);

        foreach (Disaster disaster in disasters)
        {
            DisasterTimer[disaster] = new Timer(Random.Range(disaster.FrequencyInSecondsMin, disaster.FrequencyInSecondsMax));
        }
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
        return CreateGameObject(Resources.Load<MyGameObject>(prefab), position, player, state);
    }

    public MyGameObject CreateGameObject(MyGameObject resource, Vector3 position, Player player, MyGameObjectState state)
    {
        MyGameObject myGameObject = Instantiate(resource, position, resource.transform.rotation);

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
        foreach (KeyValuePair<Disaster, Timer> i in DisasterTimer)
        {
            if (i.Value.Update(Time.deltaTime) == false)
            {
                continue;
            }

            i.Value.Reset();
            i.Value.Max = Random.Range(i.Key.FrequencyInSecondsMin, i.Key.FrequencyInSecondsMax);

            CreateGameObject(i.Key, GetDisasterPosition(), GetGaiaPlayer(), MyGameObjectState.Operational);
        }
    }

    private Vector3 GetDisasterPosition()
    {
        Vector3 position = Camera.main.transform.position;

        position.x += Random.Range(-Config.DisasterRange, Config.DisasterRange);
        position.z += Random.Range(-Config.DisasterRange, Config.DisasterRange);

        return position;
    }

    private Player GetGaiaPlayer()
    {
        return GameObject.Find(Config.PlayerWorld).GetComponent<Player>();
    }

    private Dictionary<Disaster, Timer> DisasterTimer = new Dictionary<Disaster, Timer>();
}
