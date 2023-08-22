using UnityEngine;

public class WaterPositionAnywhere : ITerrainPosition
{
    public Vector3 GetPosition(Ray ray)
    {
        return new Vector3(ray.origin.x, 7.0f, ray.origin.z); // TODO: Hardcoded.
    }

    public Vector3 GetPosition(Vector3 position)
    {
        return new Vector3(position.x, 7.0f, position.z); // TODO: Hardcoded.
    }
}
