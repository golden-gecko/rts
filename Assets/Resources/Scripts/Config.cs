using System.IO;
using UnityEngine;

public class Config
{
    public class Camera
    {
        public static float MinHeight { get; } = 2.0f;
        public static float MaxHeight { get; } = 100.0f;
    }

    public class Cursor3D
    {
        public static float RotateStep { get; } = 90.0f;
        public static bool SnapToGrid { get; } = true;
    }

    public class Directory
    {
        public static string Disasters { get; } = Path.Join("Objects", "Disasters");
        public static string Plants { get; } = Path.Join("Objects", "Plants");
        public static string Rocks { get; } = Path.Join("Objects", "Rocks");
        public static string Structures { get; } = Path.Join("Objects", "Structures");
        public static string Units { get; } = Path.Join("Objects", "Units");
    }

    public class Formation
    {
        public static float Spacing { get; } = 5.0f;
    }

    public class Indicator
    {
        public static float Margin { get; } = 1.1f;
        public static float TextOffset { get; } = 2.0f;
    }

    public class Map
    {
        public static float ConstructionScale { get; } = 1.0f;
        public static float DamageFactor { get; } = 0.0002f;
        public static Color DataLayerColorOccupation { get; } = new Color(1.0f, 1.0f, 1.0f, 0.25f);
        public static Color DataLayerColorExploration { get; } = new Color(1.0f, 1.0f, 1.0f, 0.25f);
        public static Color DataLayerColorPower { get; } = new Color(1.0f, 1.0f, 0.0f, 0.25f);
        public static Color DataLayerColorPowerRelay { get; } = new Color(1.0f, 1.0f, 0.5f, 0.25f);
        public static Color DataLayerColorRadar { get; } = new Color(0.0f, 0.0f, 1.0f, 0.25f);
        public static Color DataLayerColorSight { get; } = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        public static Color DataLayerColorPassiveDamage { get; } = new Color(1.0f, 0.0f, 0.5f, 0.25f);
        public static Color DataLayerColorPassivePower { get; } = new Color(1.0f, 0.0f, 0.5f, 0.25f);
        public static Color DataLayerColorPassiveRange { get; } = new Color(1.0f, 0.0f, 0.5f, 0.25f);
        public static Color DataLayerColorEmpty { get; } = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        public static float MaxHeight { get; } = 2000.0f;
        public static float Scale { get; } = 1.0f;
        public static int Size { get; } = 512;
    }

    public class Prefab
    {
        public static string Base { get; } = Path.Join("Prefabs", "Base");
        public static string Indicators { get; } = Path.Join("UI", "Prefabs", "Indicators");
    }
}
