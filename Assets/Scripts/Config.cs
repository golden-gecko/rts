using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static class Blueprints
    {
        public static string Directory = Path.Join(Application.persistentDataPath, "Blueprints");
    }

    public static class Camera
    {
        public static float MinHeight = 2.0f;
        public static float MaxHeight = 100.0f;
    }

    public static class Cursor3D
    {
        public static float RotateStep = 90.0f;
        public static bool SnapToGrid = true;
    }

    public static class DataLayer
    {
        public static Color ColorOccupation = new Color(1.0f, 1.0f, 1.0f, 0.25f);
        public static Color ColorExploration = new Color(1.0f, 1.0f, 1.0f, 0.25f);
        public static Color ColorPower = new Color(1.0f, 1.0f, 0.0f, 0.25f);
        public static Color ColorPowerRelay = new Color(1.0f, 1.0f, 0.5f, 0.25f);
        public static Color ColorRadar = new Color(0.0f, 0.0f, 1.0f, 0.25f);
        public static Color ColorSight = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        public static Color ColorPassiveDamage = new Color(1.0f, 0.0f, 0.5f, 0.25f);
        public static Color ColorPassivePower = new Color(1.0f, 0.0f, 0.5f, 0.25f);
        public static Color ColorPassiveRange = new Color(1.0f, 0.0f, 0.5f, 0.25f);
        public static Color ColorEmpty = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    public static class Editor
    {
        public static float RotateSpeed = 3.0f;
    }

    public static class Formation
    {
        public static float Spacing = 5.0f;
    }

    public static class Indicator
    {
        public static float Margin = 1.1f;
        public static float TextOffset = 2.0f;
    }

    public static class Map
    {
        public static float ConstructionScale = 1.0f;
        public static float DamageFactor = 0.0002f;
        public static float MaxHeight = 2000.0f;
        public static float Scale = 1.0f;
        public static int Size = 512;
    }

    [field: SerializeField]
    public GameObject Base { get; private set; }

    [field: SerializeField]
    public GameObject BaseGameObject { get; private set; }

    [field: SerializeField]
    public GameObject Indicators { get; private set; }

    #region GameObjects
    [field: SerializeField]
    public List<GameObject> Disasters { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Missiles { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Plants { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Rocks { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Structures { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Units { get; private set; } = new List<GameObject>();
    #endregion

    #region Parts
    [field: SerializeField]
    public List<GameObject> Chassis { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Constructors { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Drives { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Engines { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Guns { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Shields { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Sights { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public List<GameObject> Storages { get; private set; } = new List<GameObject>();
    #endregion
}
