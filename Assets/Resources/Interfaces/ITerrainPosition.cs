using UnityEngine;

public interface ITerrainPosition
{
    Vector3 GetPosition(Ray ray, int layerMask = Physics.DefaultRaycastLayers);

    Vector3 GetPosition(Vector3 position, int layerMask = Physics.DefaultRaycastLayers);
}
