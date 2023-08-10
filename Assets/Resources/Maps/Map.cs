using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; }

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public bool GetTerrainPosition(Vector3 position, out RaycastHit hitInfo)
    {
        return Physics.Raycast(new Ray(position + Vector3.up * Config.TerrainMaxHeight, Vector3.down), out hitInfo, Config.RaycastMaxDistance, LayerMask.GetMask("Terrain"));
    }

    public bool GetWaterPosition(Vector3 position, out RaycastHit hitInfo)
    {
        return Physics.Raycast(new Ray(position + Vector3.up * Config.WaterMaxHeight, Vector3.down), out hitInfo, Config.RaycastMaxDistance, LayerMask.GetMask("Water"));
    }
}
