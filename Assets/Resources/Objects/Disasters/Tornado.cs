using UnityEngine;

public class Tornado : Disaster
{
    protected override void Awake()
    {
        base.Awake();

        float x = Position.x + Random.Range(-Config.DisasterDirection, Config.DisasterDirection);
        float z = Position.z + Random.Range(-Config.DisasterDirection, Config.DisasterDirection);

        Move(new Vector3(x, 0.0f, z));
    }
}
