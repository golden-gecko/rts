using UnityEngine;

public interface ITerrainPosition
{
    Vector3 GetPosition(Ray ray);

    Vector3 GetPosition(Vector3 position);
}
