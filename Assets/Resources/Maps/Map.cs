using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell // TODO: Optimize.
{
    public Dictionary<Player, int> VisibleByRadar = new Dictionary<Player, int>();

    public Dictionary<Player, int> VisibleByAntiRadar = new Dictionary<Player, int>();

    public Dictionary<Player, int> VisibleBySight = new Dictionary<Player, int>();
}

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

        for (int x = 0; x < Size; x++) // TODO: Depends on script execution order.
        {
            for (int z = 0; z < Size; z++)
            {
                Cells[x, z] = new Cell();
            }
        }

        Clear();
    }

    protected void Update()
    {
        Clear();
    }

    public Vector3 ValidatePosition(Vector3 position) // TODO: Remove?
    {
        Ray ray = new Ray(position + Vector3.up * Config.TerrainMaxHeight, Vector3.down);
        int mask = LayerMask.GetMask("Terrain") | LayerMask.GetMask("Water");

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.RaycastMaxDistance, mask) == false)
        {
            return Vector3.zero;
        }

        return hitInfo.point;
    }

    public bool ValidatePosition(MyGameObject myGameObject, Vector3 position, out Vector3 validated)
    {
        Ray ray = new Ray(position + Vector3.up * Config.TerrainMaxHeight, Vector3.down);
        int mask = LayerMask.GetMask("Terrain") | LayerMask.GetMask("Water");

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.RaycastMaxDistance, mask) == false)
        {
            validated = Vector3.zero;
            
            return false;
        }

        bool air = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Air);
        bool terrain = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Terrain);
        bool water = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Water);

        if (air)
        {
            validated = new Vector3(hitInfo.point.x, hitInfo.point.y + myGameObject.Altitude, hitInfo.point.z);
         
            return true;
        }

        if (terrain && water)
        {
            validated = hitInfo.point;

            return true;
        }

        if (terrain && hitInfo.transform.CompareTag("Terrain")) // TODO: Hardcoded.
        {
            validated = hitInfo.point;

            return true;
        }

        if (water && hitInfo.transform.CompareTag("Water")) // TODO: Hardcoded.
        {
            validated = hitInfo.point;

            return true;
        }

        validated = Vector3.zero;

        return false;
    }

    public void SetVisibleByRadar(MyGameObject myGameObject, float range)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((myGameObject.Position.x - range) / Scale), 0, Mathf.FloorToInt((myGameObject.Position.z - range) / Scale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((myGameObject.Position.x + range) / Scale), 0, Mathf.FloorToInt((myGameObject.Position.z + range) / Scale));

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (x < 0 || x > Size - 1)
                {
                    continue;
                }

                if (z < 0 || z > Size - 1)
                {
                    continue;
                }

                if (Cells[x, z].VisibleByRadar.ContainsKey(myGameObject.Player) == false)
                {
                    Cells[x, z].VisibleByRadar[myGameObject.Player] = 1;
                }
                else
                {
                    Cells[x, z].VisibleByRadar[myGameObject.Player] += 1;
                }
            }
        }
    }

    public void SetVisibleByAntiRadar(MyGameObject myGameObject, float range)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((myGameObject.Position.x - range) / Scale), 0, Mathf.FloorToInt((myGameObject.Position.z - range) / Scale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((myGameObject.Position.x + range) / Scale), 0, Mathf.FloorToInt((myGameObject.Position.z + range) / Scale));

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (x < 0 || x > Size - 1)
                {
                    continue;
                }

                if (z < 0 || z > Size - 1)
                {
                    continue;
                }

                if (Cells[x, z].VisibleByAntiRadar.ContainsKey(myGameObject.Player) == false)
                {
                    Cells[x, z].VisibleByAntiRadar[myGameObject.Player] = 1;
                }
                else
                {
                    Cells[x, z].VisibleByAntiRadar[myGameObject.Player] += 1;
                }
            }
        }
    }

    public void SetVisibleBySight(MyGameObject myGameObject, float range)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((myGameObject.Position.x - range) / Scale), 0, Mathf.FloorToInt((myGameObject.Position.z - range) / Scale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((myGameObject.Position.x + range) / Scale), 0, Mathf.FloorToInt((myGameObject.Position.z + range) / Scale));

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (x < 0 || x > Size - 1)
                {
                    continue;
                }

                if (z < 0 || z > Size - 1)
                {
                    continue;
                }

                if (Cells[x, z].VisibleBySight.ContainsKey(myGameObject.Player) == false)
                {
                    Cells[x, z].VisibleBySight[myGameObject.Player] = 1;
                }
                else
                {
                    Cells[x, z].VisibleBySight[myGameObject.Player] += 1;
                }
            }
        }
    }

    public bool IsVisibleByRadar(MyGameObject myGameObject, Player active)
    {
        Vector3Int position = new Vector3Int(Mathf.FloorToInt(myGameObject.Position.x / Scale), 0, Mathf.FloorToInt(myGameObject.Position.z / Scale));

        int byRadar = Cells[position.x, position.z].VisibleByRadar.ContainsKey(active) ? Cells[position.x, position.z].VisibleByRadar[active] : 0;
        int byAntiRadar = Cells[position.x, position.z].VisibleByAntiRadar.ContainsKey(myGameObject.Player) ? Cells[position.x, position.z].VisibleByAntiRadar[myGameObject.Player] : 0;

        return byRadar > byAntiRadar;
    }

    public bool IsVisibleBySight(MyGameObject myGameObject, Player active)
    {
        Vector3Int position = new Vector3Int(Mathf.FloorToInt(myGameObject.Position.x / Scale), 0, Mathf.FloorToInt(myGameObject.Position.z / Scale));

        return Cells[position.x, position.z].VisibleBySight.ContainsKey(active) && Cells[position.x, position.z].VisibleBySight[active] > 0;
    }

    protected void Clear()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int z = 0; z < Size; z++)
            {
                Player[] players1 = Cells[x, z].VisibleByRadar.Keys.ToArray();

                foreach (Player player in players1)
                {
                    Cells[x, z].VisibleByRadar[player] = 0;
                }

                Player[] players2 = Cells[x, z].VisibleByAntiRadar.Keys.ToArray();

                foreach (Player player in players2)
                {
                    Cells[x, z].VisibleByAntiRadar[player] = 0;
                }

                Player[] players3 = Cells[x, z].VisibleBySight.Keys.ToArray();

                foreach (Player player in players3)
                {
                    Cells[x, z].VisibleBySight[player] = 0;
                }
            }
        }
    }

    public ITerrainPosition CameraPositionHandler { get; } = new TerrainPositionAnywhere();

    public ITerrainPosition StructurePositionHandler { get; } = new TerrainPositionCenterGrid();

    private static float Scale = 5.0f; // TODO: Hardcoded.

    private static int Size = 100; // TODO: Hardcoded.

    private Cell[,] Cells = new Cell[Size, Size];
}
