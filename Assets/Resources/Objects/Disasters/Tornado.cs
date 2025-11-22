using UnityEngine;

public class Tornado : Disaster
{
    protected override void Awake()
    {
        base.Awake();

        float x = Position.x + Random.Range(-200.0f, 200.0f); // TODO: Hardcoded.
        float z = Position.z + Random.Range(-200.0f, 200.0f);

        Move(new Vector3(x, 0.0f, z));
    }

    protected override void Update()
    {
        base.Update();
    }
}
