using UnityEngine;

public class Tornado : Disaster
{
    protected override void Awake()
    {
        base.Awake();

        float x = Position.x + Random.Range(-100.0f, 100.0f);
        float z = Position.z + Random.Range(-100.0f, 100.0f);

        Move(new Vector3(x, 0.0f, z));
    }
}
