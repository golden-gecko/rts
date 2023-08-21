using UnityEngine;

public class WaterPositionCenterGrid : ITerrainPosition
{
    public Vector3 GetPosition(Ray ray)
    {
        float x = Mathf.Floor(ray.origin.x / Scale) * Scale + Scale / 2.0f;
        float z = Mathf.Floor(ray.origin.z / Scale) * Scale + Scale / 2.0f;

        return new Vector3(x, 7.0f, z); // TODO: Hardcoded.
    }

    public Vector3 GetPosition(Vector3 position)
    {
        float x = Mathf.Floor(position.x / Scale) * Scale + Scale / 2.0f;
        float z = Mathf.Floor(position.z / Scale) * Scale + Scale / 2.0f;

        return new Vector3(x, 7.0f, z); // TODO: Hardcoded.
    }

    public float Scale { get; } = 2.0f;
}
