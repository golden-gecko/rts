using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        for (int x = 0; x < Config.Map.VisibilitySize; x++)
        {
            for (int z = 0; z < Config.Map.VisibilitySize; z++)
            {
                Cells[x, z] = new Cell();
            }
        }

        Clear();
    }

    public void SetVisibleByRadar(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = Utils.ToGrid(new Vector3(position.x - range, 0.0f, position.z - range), Config.Map.VisibilityScale);
        Vector3Int end = Utils.ToGrid(new Vector3(position.x + range, 0.0f, position.z + range), Config.Map.VisibilityScale);

        Vector3Int center = Utils.ToGrid(position, Config.Map.VisibilityScale);
        int radius = Mathf.FloorToInt(range / Config.Map.VisibilityScale);

        Texture2D radarTexture = Radar.mainTexture as Texture2D;

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (Utils.IsPointInRect(x, z, Config.Map.VisibilitySize) == false)
                {
                    continue;
                }

                if (Utils.IsPointInCircle(x, z, center, radius) == false)
                {
                    continue;
                }

                SetVisible(Cells[x, z].VisibleByRadar, myGameObject.Player, value);

                if (HUD.Instance.ActivePlayer == myGameObject.Player)
                {
                    if (Cells[x, z].VisibleByRadar[myGameObject.Player] > (Cells[x, z].VisibleByAntiRadar.ContainsKey(myGameObject.Player) ? Cells[x, z].VisibleByAntiRadar[myGameObject.Player] : 0))
                    {
                        radarTexture.SetPixel(x, z, Config.Map.DataLayerColorRadar);
                    }
                    else
                    {
                        radarTexture.SetPixel(x, z, Config.Map.DataLayerColorEmpty);
                    }
                }
            }
        }

        radarTexture.Apply();
    }

    public void SetVisibleByAntiRadar(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = Utils.ToGrid(new Vector3(position.x - range, 0.0f, position.z - range), Config.Map.VisibilityScale);
        Vector3Int end = Utils.ToGrid(new Vector3(position.x + range, 0.0f, position.z + range), Config.Map.VisibilityScale);

        Vector3Int center = Utils.ToGrid(position, Config.Map.VisibilityScale);
        int radius = Mathf.FloorToInt(range / Config.Map.VisibilityScale);

        Texture2D radarTexture = Radar.mainTexture as Texture2D;

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (Utils.IsPointInRect(x, z, Config.Map.VisibilitySize) == false)
                {
                    continue;
                }

                if (Utils.IsPointInCircle(x, z, center, radius) == false)
                {
                    continue;
                }

                SetVisible(Cells[x, z].VisibleByAntiRadar, myGameObject.Player, value);

                if (HUD.Instance.ActivePlayer == myGameObject.Player)
                {
                    if (Cells[x, z].VisibleByRadar[myGameObject.Player] > Cells[x, z].VisibleByAntiRadar[myGameObject.Player])
                    {
                        radarTexture.SetPixel(x, z, Config.Map.DataLayerColorRadar);
                    }
                    else
                    {
                        radarTexture.SetPixel(x, z, Config.Map.DataLayerColorEmpty);
                    }
                }
            }
        }

        radarTexture.Apply();
    }

    public void SetVisibleBySight(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = Utils.ToGrid(new Vector3(position.x - range, 0.0f, position.z - range), Config.Map.VisibilityScale);
        Vector3Int end = Utils.ToGrid(new Vector3(position.x + range, 0.0f, position.z + range), Config.Map.VisibilityScale);

        Vector3Int center = Utils.ToGrid(position, Config.Map.VisibilityScale);
        int radius = Mathf.FloorToInt(range / Config.Map.VisibilityScale);

        Texture2D explorationTexture = Exploration.mainTexture as Texture2D;
        Texture2D sightTexture = Sight.mainTexture as Texture2D;

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (Utils.IsPointInRect(x, z, Config.Map.VisibilitySize) == false)
                {
                    continue;
                }

                if (Utils.IsPointInCircle(x, z, center, radius) == false)
                {
                    continue;
                }

                SetVisible(Cells[x, z].VisibleBySight, myGameObject.Player, value);

                if (value > 0)
                {
                    SetVisible(Cells[x, z].Explored, myGameObject.Player, value);

                    if (HUD.Instance.ActivePlayer == myGameObject.Player)
                    {
                        explorationTexture.SetPixel(x, z, Config.Map.DataLayerColorExploration);
                    }
                }

                if (HUD.Instance.ActivePlayer == myGameObject.Player)
                {
                    if (Cells[x, z].VisibleBySight[myGameObject.Player] > 0)
                    {
                        sightTexture.SetPixel(x, z, Config.Map.DataLayerColorSight);
                    }
                    else
                    {
                        sightTexture.SetPixel(x, z, Config.Map.DataLayerColorEmpty);
                    }
                }
            }
        }

        explorationTexture.Apply();
        sightTexture.Apply();
    }

    public void SetVisibleByPower(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = Utils.ToGrid(new Vector3(position.x - range, 0.0f, position.z - range), Config.Map.VisibilityScale);
        Vector3Int end = Utils.ToGrid(new Vector3(position.x + range, 0.0f, position.z + range), Config.Map.VisibilityScale);

        Vector3Int center = Utils.ToGrid(position, Config.Map.VisibilityScale);
        int radius = Mathf.FloorToInt(range / Config.Map.VisibilityScale);

        Texture2D powerTexture = Power.mainTexture as Texture2D;

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (x == center.x && z == center.z)
                {
                    continue;
                }

                if (Utils.IsPointInRect(x, z, Config.Map.VisibilitySize) == false)
                {
                    continue;
                }

                if (Utils.IsPointInCircle(x, z, center, radius) == false)
                {
                    continue;
                }

                SetVisible(Cells[x, z].VisibleByPower, myGameObject.Player, value);

                if (HUD.Instance.ActivePlayer == myGameObject.Player)
                {
                    if (Cells[x, z].VisibleByPower[myGameObject.Player] > 0)
                    {
                        powerTexture.SetPixel(x, z, Config.Map.DataLayerColorPower);
                    }
                    else
                    {
                        powerTexture.SetPixel(x, z, Config.Map.DataLayerColorEmpty);
                    }
                }
            }
        }

        powerTexture.Apply();
    }

    public bool IsExplored(MyGameObject myGameObject, Player active) // TODO: Why active player?
    {
        Vector3Int position = new Vector3Int(Mathf.FloorToInt(myGameObject.Position.x / Config.Map.VisibilityScale), 0, Mathf.FloorToInt(myGameObject.Position.z / Config.Map.VisibilityScale));

        return Cells[position.x, position.z].Explored.ContainsKey(active) && Cells[position.x, position.z].Explored[active] > 0;
    }

    public bool IsVisibleByRadar(MyGameObject myGameObject, Player active) // TODO: Why active player?
    {
        Vector3Int position = Utils.ToGrid(myGameObject.Position, Config.Map.VisibilityScale);

        int byRadar = Cells[position.x, position.z].VisibleByRadar.ContainsKey(active) ? Cells[position.x, position.z].VisibleByRadar[active] : 0;
        int byAntiRadar = Cells[position.x, position.z].VisibleByAntiRadar.ContainsKey(myGameObject.Player) ? Cells[position.x, position.z].VisibleByAntiRadar[myGameObject.Player] : 0;

        return byRadar > byAntiRadar;
    }

    public bool IsVisibleBySight(MyGameObject myGameObject, Player active) // TODO: Why active player?
    {
        Vector3Int position = Utils.ToGrid(myGameObject.Position, Config.Map.VisibilityScale);

        return Cells[position.x, position.z].VisibleBySight.ContainsKey(active) && Cells[position.x, position.z].VisibleBySight[active] > 0;
    }

    public bool IsVisibleByPower(MyGameObject myGameObject)
    {
        Vector3Int position = Utils.ToGrid(myGameObject.Position, Config.Map.VisibilityScale);

        return Cells[position.x, position.z].VisibleByPower.ContainsKey(myGameObject.Player) && Cells[position.x, position.z].VisibleByPower[myGameObject.Player] > 0;
    }

    public bool IsVisibleByPowerRelay(MyGameObject myGameObject)
    {
        Vector3Int position = Utils.ToGrid(myGameObject.Position, Config.Map.VisibilityScale);

        return Cells[position.x, position.z].VisibleByPowerRelay.ContainsKey(myGameObject.Player) && Cells[position.x, position.z].VisibleByPowerRelay[myGameObject.Player] > 0;
    }

    public bool GetPosition(MyGameObject myGameObject, Vector3 origin, out Vector3 position, out MyGameObjectMapLayer mapLayer)
    {
        RaycastHit[] hits = Utils.RaycastAllFromTop(origin, Utils.GetMapMask());

        if (hits.Length <= 0)
        {
            position = Vector3.zero;
            mapLayer = MyGameObjectMapLayer.None;

            return false;
        }

        Vector3 terrainPosition = Vector3.zero;
        Vector3 waterPosition = Vector3.zero;

        foreach (RaycastHit hitInfo in hits)
        {
            if (Utils.IsTerrain(hitInfo))
            {
                terrainPosition = hitInfo.point;
            }
            else if (Utils.IsWater(hitInfo))
            {
                waterPosition = hitInfo.point;
            }
        }

        bool underwater = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Underwater);

        if (terrainPosition.y > waterPosition.y)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Terrain;

            return true;
        }

        if (underwater)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Underwater;

            return true;
        }

        position = waterPosition;
        mapLayer = MyGameObjectMapLayer.Water;

        return true;
    }

    public bool GetPositionForCamera(Vector3 origin, out Vector3 position, out MyGameObjectMapLayer mapLayer)
    {
        RaycastHit[] hits = Utils.RaycastAllFromTop(origin, Utils.GetMapMask());

        if (hits.Length <= 0)
        {
            position = Vector3.zero;
            mapLayer = MyGameObjectMapLayer.None;

            return false;
        }

        Vector3 terrainPosition = Vector3.zero;
        Vector3 waterPosition = Vector3.zero;

        foreach (RaycastHit hitInfo in hits)
        {
            if (Utils.IsTerrain(hitInfo))
            {
                terrainPosition = hitInfo.point;
            }
            else if (Utils.IsWater(hitInfo))
            {
                waterPosition = hitInfo.point;
            }
        }

        if (terrainPosition.y > waterPosition.y)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Terrain;

            return true;
        }

        position = waterPosition;
        mapLayer = MyGameObjectMapLayer.Water;

        return true;
    }

    public bool MouseToPosition(MyGameObject myGameObject, out Vector3 position, out MyGameObjectMapLayer mapLayer)
    {
        RaycastHit[] hits = Utils.RaycastAllFromMouse(Utils.GetMapMask());

        if (hits.Length <= 0)
        {
            position = Vector3.zero;
            mapLayer = MyGameObjectMapLayer.None;

            return false;
        }

        Vector3 terrainPosition = Vector3.zero;
        Vector3 waterPosition = Vector3.zero;

        foreach (RaycastHit hitInfo in hits)
        {
            if (Utils.IsTerrain(hitInfo))
            {
                terrainPosition = hitInfo.point;
            }
            else if (Utils.IsWater(hitInfo))
            {
                waterPosition = hitInfo.point;
            }
        }

        bool underwater = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Underwater);

        if (terrainPosition.y > waterPosition.y)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Terrain;

            return true;
        }

        if (underwater)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Underwater;

            return true;
        }

        position = waterPosition;
        mapLayer = MyGameObjectMapLayer.Water;

        return true;
    }

    public bool ValidatePosition(MyGameObject myGameObject, Vector3 position, out Vector3 validated)
    {
        RaycastHit[] hits = Utils.RaycastAllFromTop(position, Utils.GetMapMask());

        if (hits.Length <= 0)
        {
            validated = Vector3.zero;

            return false;
        }

        Vector3 terrainPosition = Vector3.zero;
        Vector3 waterPosition = Vector3.zero;

        foreach (RaycastHit hitInfo in hits)
        {
            if (Utils.IsTerrain(hitInfo))
            {
                terrainPosition = hitInfo.point;
            }
            else if (Utils.IsWater(hitInfo))
            {
                waterPosition = hitInfo.point;
            }
        }

        bool air = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Air);
        bool hover = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Hover);
        bool submerged = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Submerged);
        bool terrain = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Terrain);
        bool underwater = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Underwater);
        bool water = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Water);

        // Any position in air).
        if (air)
        {
            validated = position;

            return true;
        }

        // Position in air above terrain or water).
        if (hover)
        {
            if (terrainPosition.y > waterPosition.y)
            {
                validated = new Vector3(terrainPosition.x, terrainPosition.y + myGameObject.Altitude, terrainPosition.z);
            }
            else
            {
                validated = new Vector3(waterPosition.x, waterPosition.y + myGameObject.Altitude, waterPosition.z);
            }

            return true;
        }

        // Position under water surface.
        if (submerged)
        {
            if (terrainPosition.y > waterPosition.y)
            {
                validated = Vector3.zero;

                return false;
            }
            else
            {
                validated = new Vector3(waterPosition.x, waterPosition.y - myGameObject.Depth, waterPosition.z);

                return true;
            }
        }

        // Position on terrain or underwater.
        if (terrain && underwater)
        {
            validated = new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z);

            return true;
        }

        // Position on terrain or water.
        if (terrain && water)
        {
            if (terrainPosition.y > waterPosition.y)
            {
                validated = new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z);

                return true;
            }
            else
            {
                validated = new Vector3(waterPosition.x, waterPosition.y, waterPosition.z);

                return true;
            }
        }

        // Position on terrain.
        if (terrain)
        {
            if (terrainPosition.y > waterPosition.y)
            {
                validated = new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z);

                return true;
            }
            else
            {
                validated = Vector3.zero;

                return false;
            }
        }

        // Position on water.
        if (water)
        {
            if (waterPosition.y > terrainPosition.y)
            {
                validated = new Vector3(waterPosition.x, waterPosition.y, waterPosition.z);

                return true;
            }
            else
            {
                validated = Vector3.zero;

                return false;
            }
        }

        validated = Vector3.zero;

        return false;
    }

    private void Clear()
    {
        for (int x = 0; x < Config.Map.VisibilitySize; x++)
        {
            for (int z = 0; z < Config.Map.VisibilitySize; z++)
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

                Player[] players4 = Cells[x, z].VisibleByPower.Keys.ToArray();

                foreach (Player player in players4)
                {
                    Cells[x, z].VisibleByPower[player] = 0;
                }

                Player[] players5 = Cells[x, z].VisibleByPowerRelay.Keys.ToArray();

                foreach (Player player in players5)
                {
                    Cells[x, z].VisibleByPowerRelay[player] = 0;
                }
            }
        }

        ClearTexture(Exploration.mainTexture as Texture2D);
        ClearTexture(Power.mainTexture as Texture2D);
        ClearTexture(Radar.mainTexture as Texture2D);
        ClearTexture(Sight.mainTexture as Texture2D);
        ClearTexture(Sight.mainTexture as Texture2D);
    }

    private void ClearTexture(Texture2D texture)
    {
        Color[] colors = new Color[texture.width * texture.height];
        Array.Fill(colors, new Color(1.0f, 1.0f, 1.0f, 0.0f));

        texture.SetPixels(0, 0, texture.width, texture.height, colors);
        texture.Apply();
    }

    private void SetVisible(Dictionary<Player, int> layer, Player player, int value)
    {
        if (layer.ContainsKey(player) == false)
        {
            layer[player] = value;
        }
        else
        {
            layer[player] += value;
        }
    }

    [field: SerializeField]
    public Material Exploration { get; private set; }

    [field: SerializeField]
    public Material Power { get; private set; }

    [field: SerializeField]
    public Material Radar { get; private set; }

    [field: SerializeField]
    public Material Sight { get; private set; }

    private Cell[,] Cells { get; set; } = new Cell[Config.Map.VisibilitySize, Config.Map.VisibilitySize];
}
