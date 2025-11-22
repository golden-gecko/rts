using UnityEngine;

public class WaterPositionCenterGrid : ITerrainPosition
{
    public Vector3 GetPosition(Ray ray)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.RaycastMaxDistance, LayerMask.GetMask("Water")) == false)
        {
            return Vector3.zero;
        }

        float x = Mathf.Floor(hitInfo.point.x / Scale) * Scale + Scale / 2.0f;
        float z = Mathf.Floor(hitInfo.point.z / Scale) * Scale + Scale / 2.0f;

        return new Vector3(x, hitInfo.point.y, z);
    }

    public Vector3 GetPosition(Vector3 position)
    {
        return GetPosition(new Ray(position + Vector3.up * Config.TerrainMaxHeight, Vector3.down));
    }

    public float Scale { get; } = 2.0f;
}
