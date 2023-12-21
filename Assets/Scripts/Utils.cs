using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Utils
{
    #region Blueprints
    public static List<Blueprint> CreateBlueprints()
    {
        List<Blueprint> blueprints = new List<Blueprint>();

        if (Directory.Exists(Config.Blueprints.Directory) == false)
        {
            return blueprints;
        }

        foreach (string file in Directory.EnumerateFiles(Config.Blueprints.Directory))
        {
            if (Path.GetExtension(file).ToLower() != ".json")
            {
                continue;
            }

            blueprints.Add(JsonUtility.FromJson<Blueprint>(File.ReadAllText(file)));
        }

        return blueprints;
    }
    #endregion

    #region Enum
    public static string[] GetFormationNames()
    {
        string[] names = System.Enum.GetNames(typeof(Formation));

        System.Array.Sort(names);

        return names;
    }

    public static string[] GetOrderNames()
    {
        string[] names = System.Enum.GetNames(typeof(OrderType));

        System.Array.Sort(names);

        return names;
    }
    #endregion

    #region Grid
    public static float SnapToCorner(float value, float scale)
    {
        return Mathf.Round(value / scale) * scale;
    }

    public static Vector3 SnapToCorner(Vector3 position, float scale)
    {
        return new Vector3(SnapToCorner(position.x, scale), position.y, SnapToCorner(position.z, scale));
    }

    public static Vector3 SnapToGrid(Vector3 position, Vector3Int sizeGrid, float scale, bool yAxis = false)
    {
        float x;
        float y;
        float z;

        if (sizeGrid.x % 2 == 0)
        {
            x = SnapToCorner(position.x, scale);
        }
        else
        {
            x = SnapToCenter(position.x, scale);
        }

        if (yAxis)
        {
            if (sizeGrid.y % 2 == 0)
            {
                y = SnapToCorner(position.y, scale);
            }
            else
            {
                y = SnapToCenter(position.y, scale);
            }
        }
        else
        {
            y = position.y;
        }

        if (sizeGrid.z % 2 == 0)
        {
            z = SnapToCorner(position.z, scale);
        }
        else
        {
            z = SnapToCenter(position.z, scale);
        }

        return new Vector3(x, y, z);
    }

    public static float SnapToCenter(float value, float scale)
    {
        return Mathf.Floor(value / scale) * scale + scale / 2.0f;
    }

    public static Vector3 SnapToCenter(Vector3 position, float scale)
    {
        return new Vector3(SnapToCenter(position.x, scale), position.y, SnapToCenter(position.z, scale));
    }

    public static int ToGrid(float value, float scale)
    {
        return Mathf.RoundToInt(value / scale);
    }

    public static Vector3Int ToGrid(Vector3 position, float scale)
    {
        return new Vector3Int(ToGrid(position.x, scale), 0, ToGrid(position.z, scale));
    }
    #endregion

    #region Instantiate
    public static MyGameObject CreateGameObject(string prefab, Vector3 position, Quaternion rotation, Player player, MyGameObjectState state)
    {
        Game.Instance.GetGameObject(prefab, out MyGameObject myGameObject, out Blueprint blueprint);

        if (myGameObject != null)
        {
            return CreateGameObject(myGameObject, position, rotation, player, state);
        }

        if (blueprint != null)
        {
            return CreateGameObject(blueprint, position, rotation, player, state);
        }

        return null;
    }

    public static MyGameObject CreateGameObject(MyGameObject resource, Vector3 position, Quaternion rotation, Player player, MyGameObjectState state)
    {
        MyGameObject myGameObject = Object.Instantiate(resource, position, rotation);

        myGameObject.SetPlayer(player);
        myGameObject.SetState(state);

        switch (state)
        {
            case MyGameObjectState.UnderAssembly:
            case MyGameObjectState.UnderConstruction:
                // TODO: TEMP
                /*
                foreach (Part part in myGameObject.GetComponentsInChildren<Part>())
                {
                    part.ConstructionResources.RemoveAll();
                }
                */
                // TEMP
                break;
        }

        return myGameObject;
    }

    public static MyGameObject CreateGameObject(Blueprint blueprint, Vector3 position, Quaternion rotation, Player player, MyGameObjectState state, Transform parent = null)
    {
        Quaternion rotationFromParent = Quaternion.identity;

        if (parent)
        {
            rotationFromParent = ResetRotation(parent);
        }

        // TODO: Refactor. Remove Game singleton.
        // Game.Instance.Config.BaseGameObject
        MyGameObject myGameObject = Object.Instantiate(GameObject.Find("Setup").GetComponent<Config>().BaseGameObject, position, rotation, parent).GetComponent<MyGameObject>();

        myGameObject.name = blueprint.Name;
        myGameObject.SetPlayer(player);
        myGameObject.SetState(state);

        foreach (BlueprintComponent i in blueprint.Parts)
        {
            if (i.Part == null)
            {
                i.Part = LoadPart(i.PartType, i.Name);

                if (i.Part == null)
                {
                    continue;
                }
            }

            i.Instance = Object.Instantiate(i.Part, myGameObject.transform.Find("Body").transform); // TODO: myGameObject.Body.transform
            i.Instance.transform.localPosition = i.Position;
        }

        if (parent)
        {
            RestoreRotation(parent, rotationFromParent);
        }

        return myGameObject;
    }

    public static GameObject LoadPart(PartType partType, string name)
    {
        // TODO: Refactor. Remove Game singleton.
        // Game.Instance.Config
        Config config = GameObject.Find("Setup").GetComponent<Config>();

        switch (partType)
        {
            case PartType.Chassis:
                return config.Chassis.Find(x => x.name == name);

            case PartType.Constructor:
                return config.Constructors.Find(x => x.name == name);

            case PartType.Drive:
                return config.Drives.Find(x => x.name == name);

            case PartType.Engine:
                return config.Engines.Find(x => x.name == name);

            case PartType.Gun:
                return config.Guns.Find(x => x.name == name);

            case PartType.Radar:
                return config.Radars.Find(x => x.name == name);

            case PartType.Shield:
                return config.Shields.Find(x => x.name == name);

            case PartType.Sight:
                return config.Sights.Find(x => x.name == name);

            case PartType.Storage:
                return config.Storages.Find(x => x.name == name);
        }

        return null;
    }
    #endregion

    #region Mask
    public static int GetGameObjectMask()
    {
        return LayerMask.GetMask("Foundation") | LayerMask.GetMask("GameObject");
    }

    public static int GetMapMask()
    {
        return /* TODO: LayerMask.GetMask("Foundation") | */ LayerMask.GetMask("Terrain") | LayerMask.GetMask("Water");
    }

    public static int GetTerrainMask()
    {
        return /* TODO: LayerMask.GetMask("Foundation") | */ LayerMask.GetMask("Terrain");
    }
    #endregion

    #region Math
    public static float Angle(Vector3 value)
    {
        float angle = Vector3.Angle(value, Vector3.forward);

        return (Vector3.Angle(Vector3.right, value) > 90.0f) ? 360.0f - angle : angle;
    }

    public static bool IsCloseTo(Vector3 source, Vector3 target)
    {
        return IsInRange(source, target, 0.0f, 1.0f);
    }

    public static bool IsInRange(Vector3 source, Vector3 target, float rangeMax)
    {
        return IsInRange(source, target, 0.0f, rangeMax);
    }

    public static bool IsInRange(Vector3 source, Vector3 target, float rangeMin, float rangeMax)
    {
        Vector3 a = source;
        Vector3 b = target;

        a.y = 0.0f;
        b.y = 0.0f;

        float magnitude = (b - a).magnitude;

        return rangeMin <= magnitude && magnitude <= rangeMax;
    }

    public static bool IsPointInCircle(int x, int z, Vector3Int center, int radius)
    {
        int dx = Mathf.Abs(x - center.x);
        int dz = Mathf.Abs(z - center.z);

        if (dx + dz < radius)
        {
            return true;
        }

        if (dx > radius)
        {
            return false;
        }

        if (dz > radius)
        {
            return false;
        }

        return dx * dx + dz * dz < radius * radius;
    }

    public static bool IsPointInRect(int x, int z, int size)
    {
        if (x < 0 || x > size - 1)
        {
            return false;
        }

        if (z < 0 || z > size - 1)
        {
            return false;
        }

        return true;
    }

    public static int MakeEven(int value)
    {
        return value % 2 == 0 ? value : value + 1;
    }

    public static int MakeOdd(int value)
    {
        return value % 2 == 0 ? value + 1 : value;
    }
    #endregion

    #region Raycast
    public static MyGameObject RaycastGameObjectFromMouse()
    {
        RaycastHit hitInfo;

        if (Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, GetGameObjectMask()) == false)
        {
            return null;
        }

        MyGameObject myGameObject = GetGameObject(hitInfo);

        if (myGameObject == null)
        {
            return null;
        }

        if (myGameObject.VisibilityState != MyGameObjectVisibilityState.Visible)
        {
            return null;
        }

        return myGameObject;
    }

    public static bool RaycastFromMouse(out RaycastHit hitInfo, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, layerMask);
    }

    public static RaycastHit[] RaycastAllFromMouse(int layerMask = Physics.DefaultRaycastLayers)
    {
        return RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), layerMask);
    }

    public static bool RaycastFromTop(Vector3 position, out RaycastHit hitInfo, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Raycast(new Ray(new Vector3(position.x, Config.Map.MaxHeight, position.z), Vector3.down), out hitInfo, layerMask);
    }

    public static RaycastHit[] RaycastAllFromTop(Vector3 position, int layerMask = Physics.DefaultRaycastLayers, RaycastSortOrder sortOrder = RaycastSortOrder.None)
    {
        RaycastHit[] hits = RaycastAll(new Ray(new Vector3(position.x, Config.Map.MaxHeight, position.z), Vector3.down), layerMask);

        if (sortOrder == RaycastSortOrder.Ascending)
        {
            System.Array.Sort(hits, (a, b) => a.distance > b.distance ? -1 : 1);
        }
        else if (sortOrder == RaycastSortOrder.Descending)
        {
            System.Array.Sort(hits, (a, b) => a.distance < b.distance ? -1 : 1);
        }

        return hits;
    }

    public static bool Raycast(Ray ray, out RaycastHit hitInfo, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Physics.Raycast(ray, out hitInfo, float.MaxValue, layerMask);
    }

    public static RaycastHit[] RaycastAll(Ray ray, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Physics.RaycastAll(ray, float.MaxValue, layerMask);
    }

    public static RaycastHit[] SphereCastAll(Vector3 position, float range, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Physics.SphereCastAll(position, range, Vector3.down, 0.0f, layerMask);
    }
    #endregion

    #region Resources
    public static Texture2D GetPortrait(string name)
    {
        return Resources.Load<Texture2D>(Path.Join("Portraits", name));
    }
    #endregion

    #region Rotation
    public static Quaternion ResetRotation(Transform transform)
    {
        Quaternion rotation = transform.rotation;
        transform.rotation = Quaternion.identity;
        Physics.SyncTransforms();

        return rotation;
    }

    public static Quaternion ResetRotation(MyGameObject myGameObject)
    {
        Quaternion rotation = myGameObject.Rotation;
        myGameObject.Rotation = Quaternion.identity;
        Physics.SyncTransforms();

        return rotation;
    }

    public static void RestoreRotation(Transform transform, Quaternion rotation)
    {
        transform.rotation = rotation;
        Physics.SyncTransforms();
    }

    public static void RestoreRotation(MyGameObject myGameObject, Quaternion rotation)
    {
        myGameObject.Rotation = rotation;
        Physics.SyncTransforms();
    }
    #endregion

    #region String
    public static string FormatName(string name)
    {
        string formatted = "";

        foreach (char c in name)
        {
            if (c >= 'A' && c <= 'Z')
            {
                formatted += ' ';
                formatted += c;
            }
            else if (c == '_')
            {
                formatted += ' ';
            }
            else
            {
                formatted += c;
            }
        }

        return formatted.Trim();
    }
    #endregion

    #region Time
    public static float GetTimeFromUsage(int sum, int usage)
    {
        return Mathf.Ceil((float)sum / (float)usage);
    }
    #endregion

    public static Color32[] CreateColorArray(int width, int height, byte r, byte g, byte b, byte a)
    {
        Color32[] colors = new Color32[width * height];

        System.Array.Fill(colors, new Color32(r, g, b, a));

        return colors;
    }

    public static MyGameObject GetGameObject(Collision collision)
    {
        return collision.collider.GetComponentInParent<MyGameObject>();
    }

    public static MyGameObject GetGameObject(RaycastHit hitInfo)
    {
        return hitInfo.transform.GetComponentInParent<MyGameObject>();
    }

    public static bool IsTerrain(Collision collision)
    {
        return collision.collider.gameObject.layer == LayerMask.NameToLayer("Foundation") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Terrain");
    }

    public static bool IsWater(Collision collision)
    {
        return collision.collider.gameObject.layer == LayerMask.NameToLayer("Water");
    }

    public static bool IsTerrain(RaycastHit hitInfo)
    {
        return hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Foundation") || hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Terrain");
    }

    public static bool IsWater(RaycastHit hitInfo)
    {
        return hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Water");
    }
}
