using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    void Awake()
    {
        foreach (Disaster disaster in Resources.LoadAll<Disaster>(Config.DirectoryDisasters))
        {
            DisasterTimer[disaster] = new Timer(Random.Range(disaster.FrequencyInSecondsMin, disaster.FrequencyInSecondsMax));
        }
    }

    void Update()
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

            Utils.CreateGameObject(i.Key, GetDisasterPosition(), Player, MyGameObjectState.Operational);
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
    public Player Player { get; set; }

    [field: SerializeField]
    public float DisasterDirection { get; set; } = 200.0f;

    [field: SerializeField]
    public float DisasterRange { get; set; } = 20.0f;

    private Dictionary<Disaster, Timer> DisasterTimer = new Dictionary<Disaster, Timer>();
}
