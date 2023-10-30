using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisasterManager : Singleton<DisasterManager>
{
    protected override void Awake()
    {
        base.Awake();

        CreateTimers();
    }

    private void Update()
    {
        CreateDisaster();
    }

    private void CreateDisaster()
    {
        if (Player == null)
        {
            return;
        }

        foreach (KeyValuePair<Disaster, Timer> i in DisasterTimer)
        {
            if (i.Value.Update(Time.deltaTime) == false)
            {
                continue;
            }

            i.Value.Reset();
            i.Value.Max = Random.Range(i.Key.FrequencyInSecondsMin, i.Key.FrequencyInSecondsMax);

            Utils.CreateGameObject(i.Key, GetDisasterPosition(), Quaternion.identity, Player, MyGameObjectState.Operational);
        }
    }

    private void CreateTimers()
    {
        foreach (Disaster disaster in ConfigPrefabs.Instance.Disasters.Select(x => x.GetComponent<Disaster>()))
        {
            DisasterTimer[disaster] = new Timer(Random.Range(disaster.FrequencyInSecondsMin, disaster.FrequencyInSecondsMax));
        }
    }

    private Vector3 GetDisasterPosition()
    {
        Vector3 position = Camera.main.transform.position;

        position.x += Random.Range(-DisasterRange, DisasterRange);
        position.z += Random.Range(-DisasterRange, DisasterRange);

        return position;
    }

    [field: SerializeField]
    public Player Player { get; private set; }

    [field: SerializeField]
    public float DisasterDirection { get; private set; } = 200.0f;

    [field: SerializeField]
    public float DisasterRange { get; private set; } = 20.0f;

    private Dictionary<Disaster, Timer> DisasterTimer { get; } = new Dictionary<Disaster, Timer>();
}
